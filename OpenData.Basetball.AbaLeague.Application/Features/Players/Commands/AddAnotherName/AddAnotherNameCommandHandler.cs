using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.AddAnotherName
{
    public class AddAnotherNameCommandHandler : ICommandHandler<AddAnotherNameCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAnotherNameCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(AddAnotherNameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(!await _unitOfWork.PlayerRepository.AddAnotherName(request.PlayerId, request.Name, cancellationToken))
                {
                    return Result.Failure(new Error("100", "Unable to save antoher name for player"));
                }

                await _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                return Result.Failure(new Error("100", "Problem on infrastructure"));
            }

            return Result.Success();
        }
    }
}
