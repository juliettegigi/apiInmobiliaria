
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace proyecto1.Clases;

public class Mensaje
{





public static void enviarClave(){
           /*      var message = new MimeKit.MimeMessage();
				message.To.Add(new MailboxAddress(perfil.Nombre, perfil.Email));
				message.From.Add(new MailboxAddress("Sistema", config["SMTPUser"]));// quién lo envía
                //config["SMTPUser"]  está en el secret
				message.Subject = "Prueba de Correo desde API";
				message.Body = new TextPart("html") // usar una vista
				{
					Text = @$"<h1>Hola</h1>
					<p>¡Bienvenido, {perfil.Nombre}!</p>",//falta enviar la clave generada (sin hashear)
				};
				message.Headers.Add("Encabezado", "Valor");//solo si hace falta, por si necesito una retroalimentación de si el correo fue enviado o no
				message.Headers.Add("Otro", config["Valor"]);//otro ejemplo
				MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,  //para q no valide el certificado
					System.Security.Cryptography.X509Certificates.X509Certificate certificate,
					System.Security.Cryptography.X509Certificates.X509Chain chain,
					System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
				{ return true; };
				client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(config["SMTPUser"], config["SMTPPass"]);//estas credenciales deben estar en el user secrets
				await client.SendAsync(message); */
                }
}