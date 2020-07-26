using Institute.Common.Entities.Exam;
using Institute.Domain;
using Institute.Domain.Exam;
using Institute.Web.Infrastrcuture;
using Institute.Web.Infrastrcuture.Filters;
using Institute.Web.Models.Exam;
using Institute.Web.Security.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using static Institute.Common.AppEnums;

namespace Institute.Web.Controllers.Api
{
    [JwtAuthentication]
    public class SectionController : BaseApiController<Section,SectionVM>
    {
        protected override IBusinessRule<Section> CreateRule() => new SectionBusinessRule();
        
		[Authorize(Roles = "HumanResource")]
        [HttpPost]
		public async Task<HttpResponseMessage> GetSectionList()
		{
			try
			{
                List<SectionVM> result = null;

				var identity = RequestContext.Principal.Identity as ClaimsIdentity;
                var isInRole = RequestContext.Principal.IsInRole("Manager");
                var token = Token;
                var user = CurrentUser;

                //var sectionRule = new SectionBusinessRule();
                var sections = await BusinessRule.Queryable().ToListAsync();
                if(sections!=null)
                {
                    result = sections.Select(e => new SectionVM
                    {
                         ID=e.ID,
                         Name=e.Name,
                         Description=e.Description,
                         IsDeleted=e.IsDeleted,
                    }).ToList();
                }
             
				return Success(result);
			}
			catch (Exception ex)
			{
				return await HandleExceptionAsync(ex);
			}
		}

    }
}
