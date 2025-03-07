﻿namespace FoodApp.Api.VerticalSlicing.Features.Account.Login.Commands;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

public class LoginCommandHandler : BaseRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public LoginCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

    public override async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
        }

        var userResult = await _mediator.Send(new GetUserByEmailQuery(request.Email));

        if (!userResult.IsSuccess)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
        }

        var user = userResult.Data.Include(u => u.UserRoles)
                                                 .ThenInclude(ur => ur.Role)
                                                 .Include(u => u.RefreshTokens)
                                                 .AsTracking()
                                                 .FirstOrDefault(); ;
        if (user is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.UserNotFound);
        }
        if (!PasswordHasher.checkPassword(request.Password, user.PasswordHash) || !user.IsEmailVerified)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
        }

        var token = TokenGenerator.GenerateToken(user);

        var loginResponse = new LoginResponse()
        { 
            Id = user.Id,
            Email = user.Email,
            Token = token,
        };

        var userRepo = _unitOfWork.Repository<User>();
        var refreshTokensResult = await _mediator.Send(new GetUserActiveRefreshTokensQuery(user.Id));

        if (refreshTokensResult.IsSuccess)
        {
            var activeRefreshToken = user.RefreshTokens!.FirstOrDefault(r => r.IsActive);
            loginResponse.RefreshToken = activeRefreshToken!.Token;
        }
        else
        {
            var newRefreshToken = TokenGenerator.GenerateRefreshToken();
            user.RefreshTokens!.Add(newRefreshToken);
            userRepo.Update(user);
            await userRepo.SaveChangesAsync();
            loginResponse.RefreshToken = newRefreshToken.Token;
        }

        return Result.Success(loginResponse);
    }
}