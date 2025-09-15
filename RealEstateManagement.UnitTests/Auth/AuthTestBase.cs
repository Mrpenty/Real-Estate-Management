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
        protected Mock<UserManager<ApplicationUser>> UserManagerMock = null!;
        protected Mock<SignInManager<ApplicationUser>> SignInManagerMock = null!;
        protected Mock<ITokenRepository> TokenRepoMock = null!;
        protected DefaultHttpContext HttpContext = null!;

        // System under test
        protected AuthService Svc = null!;

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
            normalizer.Setup(n => n.NormalizeName(It.IsAny<string>())).Returns<string>(s => s?.ToUpperInvariant());
            normalizer.Setup(n => n.NormalizeEmail(It.IsAny<string>())).Returns<string>(s => s?.ToUpperInvariant());

            UserManagerMock = new Mock<UserManager<ApplicationUser>>(
                store.Object,
                identityOptions,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                normalizer.Object,
                new IdentityErrorDescriber(),
                null!,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            // ----- HttpContext -----
            HttpContext = new DefaultHttpContext();
            var httpAccessor = new Mock<IHttpContextAccessor>();
            httpAccessor.SetupGet(h => h.HttpContext).Returns(HttpContext);

            // ----- SignInManager -----
            var principalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            SignInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                UserManagerMock.Object,
                httpAccessor.Object,
                principalFactory.Object,
                identityOptions,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);

            // ----- TokenRepo -----
            TokenRepoMock = new Mock<ITokenRepository>();

            // ----- SUT -----
            Svc = new AuthService(
                UserManagerMock.Object,
                SignInManagerMock.Object,
                httpAccessor.Object,
                TokenRepoMock.Object,
                mailService: null!,
                smsService: null!,
                logger: null!,
                configuration: null!,
                walletService: null!);
        }

        /// <summary>
        /// Giả lập danh sách Users cho UserManager.Users (EF Core async queryable).
        /// </summary>
        protected void SetUsers(params ApplicationUser[] users)
        {
            var q = new TestAsyncEnumerable<ApplicationUser>(users ?? Array.Empty<ApplicationUser>());
            UserManagerMock.SetupGet(um => um.Users).Returns(q);
        }

        protected void SetupPasswordSignIn(ApplicationUser user, string password, SignInResult result)
        {
            SignInManagerMock
                .Setup(s => s.PasswordSignInAsync(user, password, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(result);
        }
        protected class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;
            public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

            public IQueryable CreateQuery(Expression expression)
            {
                var elementType = expression.Type.GetGenericArguments().FirstOrDefault() ?? typeof(TEntity);
                var q = _inner.CreateQuery(expression);
                var wrapperType = typeof(TestAsyncEnumerable<>).MakeGenericType(elementType);
                return (IQueryable)Activator.CreateInstance(wrapperType, q)!;
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
                => new TestAsyncEnumerable<TElement>(_inner.CreateQuery<TElement>(expression));

            public object Execute(Expression expression) => _inner.Execute(expression)!;

            public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression)!;

            // EF Core async
            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
            {
                var expected = typeof(TResult);

                // Trường hợp EF gọi ...Async() và chờ Task<T>
                if (expected.IsGenericType && expected.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var innerType = expected.GetGenericArguments()[0];

                    // Gọi overload non-generic => object
                    var resultObj = _inner.Execute(expression);   // tránh reflection AmbiguousMatch

                    // Task.FromResult<T>(object)
                    var fromResult = typeof(Task)
                        .GetMethods()
                        .First(m => m.Name == nameof(Task.FromResult) && m.IsGenericMethodDefinition)
                        .MakeGenericMethod(innerType);

                    var task = fromResult.Invoke(null, new[] { resultObj });
                    return (TResult)task!;
                }

                // Trường hợp EF (hiếm) chờ Task không generic
                if (expected == typeof(Task))
                {
                    _inner.Execute(expression);
                    return (TResult)(object)Task.CompletedTask;
                }

                // Đồng bộ: trả về T trực tiếp
                var syncResult = _inner.Execute(expression);
                return (TResult)syncResult!;
            }


            public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
                => new TestAsyncEnumerable<TResult>(_inner.CreateQuery<TResult>(expression));
        }


        protected class TestAsyncEnumerable<T> : IAsyncEnumerable<T>, IQueryable<T>, IEnumerable<T>
        {
            private readonly IQueryable<T> _inner;
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : this(enumerable.AsQueryable()) { }
            public TestAsyncEnumerable(IQueryable<T> inner) => _inner = inner;

            public Type ElementType => _inner.ElementType;
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
            public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

            public T Current => _inner.Current;
            public ValueTask DisposeAsync() { _inner.Dispose(); return ValueTask.CompletedTask; }
            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
        }

    }
}
