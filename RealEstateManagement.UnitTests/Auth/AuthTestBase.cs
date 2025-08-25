using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Auth
{
    /// <summary>
    /// Base test class cho AuthService. Kế thừa class này trong từng test class cho từng method.
    /// </summary>
    public abstract class AuthTestBase
    {
        // Mocks
        protected Mock<UserManager<ApplicationUser>> UserManagerMock = null!;
        protected Mock<SignInManager<ApplicationUser>> SignInManagerMock = null!;
        protected Mock<IHttpContextAccessor> HttpAccessorMock = null!;
        protected DefaultHttpContext HttpContext = null!;
        protected Mock<ITokenRepository> TokenRepoMock = null!;
        protected Mock<IMailService> MailMock = null!;
        protected Mock<ISmsService> SmsMock = null!;
        protected Mock<ILogger<AuthService>> LoggerMock = null!;
        protected Mock<IConfiguration> ConfigMock = null!;

        // System under test
        protected AuthService Svc = null!;

        // Cho phép in log ra Test Explorer Output
        public TestContext TestContext { get; set; } = null!;

        [TestInitialize]
        public virtual void Init()
        {
            // ----- UserManager -----
            var store = new Mock<IUserStore<ApplicationUser>>();
            var identityOptions = Options.Create(new IdentityOptions());
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();

            var userValidators = new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() };
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() };

            var normalizer = new Mock<ILookupNormalizer>();
            normalizer.Setup(n => n.NormalizeName(It.IsAny<string>()))
                      .Returns<string>(s => s?.ToUpperInvariant());
            normalizer.Setup(n => n.NormalizeEmail(It.IsAny<string>()))
                      .Returns<string>(s => s?.ToUpperInvariant());

            var userMgrLogger = new Mock<ILogger<UserManager<ApplicationUser>>>();

            UserManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                identityOptions,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                normalizer.Object,
                new IdentityErrorDescriber(),
                null!,
                userMgrLogger.Object);

            // ----- HttpContext / SignInManager -----
            HttpAccessorMock = new Mock<IHttpContextAccessor>();
            HttpContext = new DefaultHttpContext();
            HttpAccessorMock.SetupGet(h => h.HttpContext).Returns(HttpContext);

            var principalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var signInLogger = new Mock<ILogger<SignInManager<ApplicationUser>>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<ApplicationUser>>();

            SignInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                UserManagerMock.Object,
                HttpAccessorMock.Object,
                principalFactory.Object,
                identityOptions,
                signInLogger.Object,
                schemes.Object,
                confirmation.Object);

            // ----- Other dependencies -----
            TokenRepoMock = new Mock<ITokenRepository>();
            MailMock = new Mock<IMailService>();
            SmsMock = new Mock<ISmsService>();
            LoggerMock = new Mock<ILogger<AuthService>>();
            ConfigMock = new Mock<IConfiguration>();

            // In log ra Test Output (tuỳ chọn)
            LoggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()))
                .Callback((LogLevel level, EventId id, object state, Exception? ex, Delegate formatter) =>
                {
                    try
                    {
                        var msg = formatter.DynamicInvoke(state, ex) as string;
                        TestContext?.WriteLine($"[LOG {level}] {msg}");
                        if (ex != null) TestContext?.WriteLine($"[EXCEPTION] {ex.GetType().Name}: {ex.Message}");
                    }
                    catch { /* ignore formatting issues in tests */ }
                });

            // WalletService không dùng trong hầu hết test → có thể để null!
            WalletService wallet = null!;

            // ----- SUT -----
            Svc = new AuthService(
                UserManagerMock.Object,
                SignInManagerMock.Object,
                HttpAccessorMock.Object,
                TokenRepoMock.Object,
                MailMock.Object,
                SmsMock.Object,
                LoggerMock.Object,
                ConfigMock.Object,
                wallet);
        }

        /// <summary>
        /// Set Users cho _userManager.Users với IQueryable async (hỗ trợ FirstOrDefaultAsync).
        /// </summary>
        protected void SetUsers(params ApplicationUser[] users)
        {
            var queryable = BuildAsyncQueryable(users ?? Array.Empty<ApplicationUser>());
            UserManagerMock.SetupGet(um => um.Users).Returns(queryable);
        }
        protected static IQueryable<T> BuildAsyncQueryable<T>(IEnumerable<T> data)
        {
            var inner = data.AsQueryable();
            return new TestAsyncEnumerable<T>(inner);
        }
        /// <summary>
        /// Dựng IQueryable async cho EF Core async methods (FirstOrDefaultAsync...).
        /// </summary>
        protected static IQueryable<ApplicationUser> BuildAsyncUsers(params ApplicationUser[] users)
        {
            return new TestAsyncEnumerable<ApplicationUser>(users ?? Array.Empty<ApplicationUser>());
        }

        /// <summary>
        /// Helper setup PasswordSignInAsync theo kết quả mong muốn.
        /// </summary>
        protected void SetupPasswordSignIn(ApplicationUser user, string password, SignInResult result)
        {
            SignInManagerMock
                .Setup(s => s.PasswordSignInAsync(user, password, false, false))
                .ReturnsAsync(result);
        }

        /// <summary>
        /// Verify log theo level + message chứa chuỗi.
        /// </summary>
        protected void VerifyLogged(LogLevel level, string contains, Times times)
        {
            LoggerMock.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v!.ToString()!.Contains(contains, StringComparison.OrdinalIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times);
        }

        #region Async Queryable helpers (EF Core)
        protected class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;
            public TestAsyncQueryProvider(IQueryProvider inner) { _inner = inner; }

            public IQueryable CreateQuery(Expression expression)
                => new TestAsyncEnumerable<TEntity>(_inner.CreateQuery<TEntity>(expression));

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
                => new TestAsyncEnumerable<TElement>(_inner.CreateQuery<TElement>(expression));

            public object Execute(Expression expression)
                => _inner.Execute(expression)!;

            public TResult Execute<TResult>(Expression expression)
                => _inner.Execute<TResult>(expression)!;

            // EF Core async API
            public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
                => new TestAsyncEnumerable<TResult>(_inner.CreateQuery<TResult>(expression));

            // EF Core 7/8: return TResult (không phải Task<TResult>)
            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
                => Execute<TResult>(expression);
        }


        protected class TestAsyncEnumerable<T> : IAsyncEnumerable<T>, IQueryable<T>, IEnumerable<T>, IEnumerable
        {
            private readonly IQueryable<T> _inner;

            // NEW: ctor nhận IEnumerable -> bọc thành IQueryable
            public TestAsyncEnumerable(IEnumerable<T> items) : this(items.AsQueryable()) { }

            public TestAsyncEnumerable(IQueryable<T> inner) => _inner = inner;

            public Type ElementType => typeof(T);
            public Expression Expression => _inner.Expression;
            public IQueryProvider Provider => new TestAsyncQueryProvider<T>(_inner.Provider);

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => new TestAsyncEnumerator<T>(_inner.GetEnumerator());

            public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _inner.GetEnumerator();
        }


        protected class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;
            public TestAsyncEnumerator(IEnumerator<T> inner) { _inner = inner; }
            public T Current => _inner.Current;
            public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; }
            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
        }
        #endregion
    }
}
