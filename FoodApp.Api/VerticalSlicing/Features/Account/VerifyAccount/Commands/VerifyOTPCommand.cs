﻿namespace FoodApp.Api.VerticalSlicing.Features.Account.VerifyAccount.Commands;

public record VerifyOTPCommand(string Email, string OTP) : IRequest<Result<bool>>;

public class VerifyOTPCommandHandler : BaseRequestHandler<VerifyOTPCommand, Result<bool>>
{
    public VerifyOTPCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

    public override async Task<Result<bool>> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
    {
        var userResult = await _mediator.Send(new GetUserByEmailQuery(request.Email));
        if (!userResult.IsSuccess)
        {
            return Result.Failure<bool>(UserErrors.UserNotFound);
        }
        var user = userResult.Data.AsTracking().FirstOrDefault();

        if (user is null)
        {
            return Result.Failure<bool>(UserErrors.UserNotFound);
        }
        if (user.VerificationOTPExpiration is not null && user.VerificationOTPExpiration < DateTime.Now)
        {
            return Result.Failure<bool>(UserErrors.OTPExpired);
        }

        if (user.VerificationOTP is not null && user.VerificationOTP != request.OTP)
        {
            return Result.Failure<bool>(UserErrors.InvalidOTP);
        }

        user.VerificationOTP = null;
        user.VerificationOTPExpiration = null;
        user.IsEmailVerified = true;

        await _unitOfWork.Repository<User>().SaveChangesAsync();

        return Result.Success(true);
    }
}