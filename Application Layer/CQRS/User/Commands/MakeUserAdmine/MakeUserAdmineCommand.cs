using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Commands.MakeUserAdmine
{
    public record MakeUserAdmineCommand(int userid,int Roleid):ICommand<RequestRespones<bool>>;

    public class MakeAdmineCommandHandler : IRequestHandler<MakeUserAdmineCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.UserRole> genaricRepository;
        private readonly ILogger<MakeUserAdmineCommand> logger;

        public MakeAdmineCommandHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.UserRole> genaricRepository,ILogger<MakeUserAdmineCommand> logger)
        {
            this.genaricRepository = genaricRepository;
            this.logger = logger;
        }
        public async Task<RequestRespones<bool>> Handle(MakeUserAdmineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Userrole = new UserRole
                {
                    Userid = request.userid,
                    Roleid = request.Roleid
                };

                await genaricRepository.addAsync(Userrole);

                await genaricRepository.SaveChanges();

                return RequestRespones<bool>.Success(true, 200);
            }
            catch (Exception ex)
            {
                logger.LogError(string.Empty,ex);
               return RequestRespones<bool>.Fail($"An error occurred while making the user an admin ",400);
            }
         
        }
    }
}


