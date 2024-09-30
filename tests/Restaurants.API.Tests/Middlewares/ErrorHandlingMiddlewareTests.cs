using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Tests.Middlewares;

public class ErrorHandlingMiddlewareTests
{
	private	readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
	private readonly DefaultHttpContext _context;
	private readonly ErrorHandlingMiddleware _middleware;

    public ErrorHandlingMiddlewareTests()
    {
		_loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
		_context = new DefaultHttpContext();		
		_middleware = new ErrorHandlingMiddleware(_loggerMock.Object);
    }

    [Fact()]
	public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
	{
		// arrange

		var nextDelegateMock = new Mock<RequestDelegate>();

		// act

		await _middleware.InvokeAsync(_context, nextDelegateMock.Object);

		// assert

		nextDelegateMock.Verify(next => next.Invoke(_context), Times.Once);
	}


	[Fact()]
	public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCodeTo404()
	{
		// arrange
		var notFoundException = new NotFoundException(nameof(Restaurant), "1");

		// act
		await _middleware.InvokeAsync(_context, _ => throw notFoundException);

		// assert
		_context.Response.StatusCode.Should().Be(404);
	}

	[Fact()]
	public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
	{
		// arrange
		var forbidException = new ForbidException();

		// act
		await _middleware.InvokeAsync(_context, _ => throw forbidException);

		// assert
		_context.Response.StatusCode.Should().Be(403);
	}

	[Fact()]
	public async Task InvokeAsync_WhenUnknownExceptionThrown_ShouldSetStatusCode500()
	{
		// arrange
		var exception = new Exception();

		// act
		await _middleware.InvokeAsync(_context, _ => throw exception);

		// assert
		_context.Response.StatusCode.Should().Be(500);


	}
}