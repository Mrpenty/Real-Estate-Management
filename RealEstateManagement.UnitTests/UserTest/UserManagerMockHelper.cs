// File: UserManagerMockHelper.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;   // IAsyncQueryProvider
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace RealEstateManagement.UnitTests.UserTest
{
    public static class UserManagerMockHelper
    {
        public static Mock<UserManager<TUser>> CreateMock<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var options = Options.Create(new IdentityOptions());
            var pwdHasher = new Mock<IPasswordHasher<TUser>>().Object;
            var userValidators = new List<IUserValidator<TUser>>();
            var pwdValidators = new List<IPasswordValidator<TUser>>();
            var normalizer = new Mock<ILookupNormalizer>().Object;
            var errors = new IdentityErrorDescriber();
            var services = new Mock<IServiceProvider>().Object;
            var logger = new Mock<ILogger<UserManager<TUser>>>().Object;

            var mgr = new Mock<UserManager<TUser>>(
                store.Object,
                options,
                pwdHasher,
                userValidators,
                pwdValidators,
                normalizer,
                errors,
                services,
                logger
            );

            return mgr;
        }
    }

    // ---------- Async Queryable helpers (EF Core) ----------

    internal sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

        public IQueryable CreateQuery(Expression expression)
        {
            // ⚠️ Quan trọng: tạo TestAsyncEnumerable<> đúng kiểu phần tử của expression
            var elementType = expression.Type.GetGenericArguments().FirstOrDefault()
                              ?? typeof(TEntity); // fallback
            var asyncEnumType = typeof(TestAsyncEnumerable<>).MakeGenericType(elementType);
            return (IQueryable)Activator.CreateInstance(asyncEnumType, expression, _inner)!;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new TestAsyncEnumerable<TElement>(expression, _inner);

        public object Execute(Expression expression)
            => _inner.Execute(expression);

        public TResult Execute<TResult>(Expression expression)
            => _inner.Execute<TResult>(expression);

        // EF Core overload: trả về IAsyncEnumerable<TResult>
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
            => new TestAsyncEnumerable<TResult>(expression, _inner);

        // Tuỳ version EF Core, method này trả về TResult (không phải Task/ValueTask)
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            => _inner.Execute<TResult>(expression);
    }

    // IQueryable<T> + IAsyncEnumerable<T> (provider là LINQ-to-Objects)
    internal sealed class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        private readonly IQueryProvider _provider;
        private readonly Expression _expression;

        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
            var q = enumerable.AsQueryable();
            _provider = q.Provider;
            _expression = q.Expression;
        }

        public TestAsyncEnumerable(Expression expression, IQueryProvider provider)
            : base(expression)
        {
            _provider = provider ?? Enumerable.Empty<T>().AsQueryable().Provider;
            _expression = expression ?? Enumerable.Empty<T>().AsQueryable().Expression;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        // IQueryable<T>
        Type IQueryable.ElementType => typeof(T);
        Expression IQueryable.Expression => _expression;
        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(_provider);
    }

    internal sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

        public T Current => _inner.Current;
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
        public ValueTask DisposeAsync() { _inner.Dispose(); return default; }
    }

    public static class AsyncQueryExtensions
    {
        public static IQueryable<T> ToAsyncQueryable<T>(this IEnumerable<T> source)
            => new TestAsyncEnumerable<T>(source);
    }
}
