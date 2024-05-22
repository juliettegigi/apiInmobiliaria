
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MailKit.Net.Smtp;
using MimeKit;

using Models;

namespace proyecto1.Clases;

public class Mensaje
{
      
 
private static string GetMensaje(Propietario perfil,string enlace)
    {
       return $@"<p>Hola {perfil.Nombre}:</p>
              <p>Hemos recibido una solicitud de restablecimiento de contraseña de tu cuenta.</p>
              <p>Haz clic en el botón que aparece a continuación para cambiar tu contraseña.</p>
              <p>Ten en cuenta que este enlace es válido solo durante 24 horas. Una vez transcurrido el plazo, deberás volver a solicitar el restablecimiento de la contraseña.</p>
              <a href='{enlace}'>Cambiar contraseña</a>";
    }


public static async Task  EnviarEnlace(Propietario perfil,string subject, IConfiguration config,string enlace){
	
	Console.WriteLine("---------------------------------------------------------------------------------------");
              var message = new MimeKit.MimeMessage();
				message.To.Add(new MailboxAddress(perfil.Nombre, perfil.Email));
				message.From.Add(new MailboxAddress("jul", config["Email:SMTPUser"]));
				message.Subject = subject;
				message.Body = new TextPart("html") // usar una vista
				{ Text=Mensaje.GetMensaje(perfil,enlace)
					,//falta enviar la clave generada (sin hashear)
				};
				
				MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,  //para q no valide el certificado
					System.Security.Cryptography.X509Certificates.X509Certificate certificate,
					System.Security.Cryptography.X509Certificates.X509Chain chain,
					System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
				{ return true; };
				client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(config["Email:SMTPUser"], config["Email:SMTPPass"]);//estas credenciales deben estar en el user secrets
				await client.SendAsync(message); 
                }
}