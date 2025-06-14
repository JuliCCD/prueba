using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;

public class TestForgotPasswordModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailSender _emailSender;

    public TestForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public string Email { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = "https://prueba/reset";

        await _emailSender.SendEmailAsync(
            Input.Email,
            "Restablecer contraseña",
            $"Restablece tu contraseña haciendo clic <a href='{callbackUrl}'>aquí</a>.");

        return RedirectToPage("./ForgotPasswordConfirmation");
    }
}
namespace VIMOD.PruebasUnitarias.Identity
{
    [TestClass]
    public class ForgotPasswordModelTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<IEmailSender> _emailSenderMock;

        private TestForgotPasswordModel CreateModel()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            _emailSenderMock = new Mock<IEmailSender>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            var pageContext = new PageContext
            {
                HttpContext = httpContext
            };

            var model = new TestForgotPasswordModel(_userManagerMock.Object, _emailSenderMock.Object)
            {
                PageContext = pageContext
            };

            return model;
        }

        [TestMethod]
        public async Task RecuperarConEmailValido_EnviaCorreoYConfirma()
        {
            // Arrange
            var email = "usuario@ejemplo.com";
            var user = new IdentityUser { Email = email };

            var model = CreateModel();
            model.Input = new TestForgotPasswordModel.InputModel { Email = email };

            _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token123");
            _emailSenderMock.Setup(m => m.SendEmailAsync(email, It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            _emailSenderMock.Verify(m => m.SendEmailAsync(email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task RecuperarConEmailNoRegistrado_NoEnviaCorreoYConfirma()
        {
            // Arrange
            var email = "noexiste@ejemplo.com";
            var model = CreateModel();
            model.Input = new TestForgotPasswordModel.InputModel { Email = email };

            _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            _emailSenderMock.Verify(m => m.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task RecuperarConEmailVacio_ModelStateInvalido_NoEnviaCorreo()
        {
            // Arrange
            var model = CreateModel();
            model.ModelState.AddModelError("Input.Email", "El campo correo es obligatorio");
            model.Input = new TestForgotPasswordModel.InputModel { Email = "" };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            _emailSenderMock.Verify(m => m.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}