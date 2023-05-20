using ChatAppBackEndV1.Common.ServiceResponseResult;
using ChatAppBackEndV1.Services.UserService;
using ChatAppBackEndV2.Dtos.UserService;
using HotChocolate.Authorization;
using HotChocolate.Execution;

namespace ChatAppBackEndV2.GraphQLResolver
{
    [ExtendObjectType("Mutation")]
    public class MutationResolver
    {
        //[Authorize]
        [GraphQLName("Authenticate")]
        [GraphQLDescription("Authenticate")]
        public async Task<AuthenticateResponse> AuthenticateAsync(string username, string password, [Service]IUserService userService)
        {
            var a = await userService.AuthenticateAsync(username, password);
            if(a.IsSuccess)
            {
                return a.Result;
            }
            else
            {
                //throw new GraphQLException(new Error(a.Message, "PRODUCT_NOT_FOUND"));
                //throw (Exception)ErrorBuilder.New().SetMessage(a.Message).SetCode("NOTFOUND").Build();
                throw new QueryException(ErrorBuilder.New().SetMessage(a.Message).SetCode("NOTFOUND").Build());
            }
        }
    }
}
