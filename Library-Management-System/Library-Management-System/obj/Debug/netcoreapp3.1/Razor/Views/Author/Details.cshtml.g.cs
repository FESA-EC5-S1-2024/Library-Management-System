#pragma checksum "C:\Users\muril\Desktop\Projetos\LMS\Library-Management-System\Library-Management-System\Views\Author\Details.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "cf0d6f1f182c13f4bc85ce396a86cc11147cdcacd0a6bb9be172c695bc9ff457"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Author_Details), @"mvc.1.0.view", @"/Views/Author/Details.cshtml")]
namespace AspNetCore
{
    #line hidden
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\muril\Desktop\Projetos\LMS\Library-Management-System\Library-Management-System\Views\_ViewImports.cshtml"
using Library_Management_System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\muril\Desktop\Projetos\LMS\Library-Management-System\Library-Management-System\Views\_ViewImports.cshtml"
using Library_Management_System.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"cf0d6f1f182c13f4bc85ce396a86cc11147cdcacd0a6bb9be172c695bc9ff457", @"/Views/Author/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"622a0428b584f1aa1f3b4cda8ef31ea1d52994c7eacb33b1dd65217537edd3f1", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Author_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Library_Management_System.Models.AuthorViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"container\">\r\n    <h2>Author Details</h2>\r\n    <dl class=\"row\">\r\n        <dt class=\"col-sm-2\">Name</dt>\r\n        <dd class=\"col-sm-10\">");
#nullable restore
#line 7 "C:\Users\muril\Desktop\Projetos\LMS\Library-Management-System\Library-Management-System\Views\Author\Details.cshtml"
                         Write(Model.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\r\n\r\n        <dt class=\"col-sm-2\">Country</dt>\r\n        <dd class=\"col-sm-10\">");
#nullable restore
#line 10 "C:\Users\muril\Desktop\Projetos\LMS\Library-Management-System\Library-Management-System\Views\Author\Details.cshtml"
                         Write(Model.Country);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\r\n\r\n        <dt class=\"col-sm-2\">Birthdate</dt>\r\n        <dd class=\"col-sm-10\">");
#nullable restore
#line 13 "C:\Users\muril\Desktop\Projetos\LMS\Library-Management-System\Library-Management-System\Views\Author\Details.cshtml"
                         Write(Model.Birthdate.ToString("yyyy-MM-dd"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\r\n    </dl>\r\n</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Library_Management_System.Models.AuthorViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
