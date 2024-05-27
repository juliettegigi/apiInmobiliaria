
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MailKit.Net.Smtp;
using MimeKit;

using Models;
using proyecto1.Clases;

namespace proyecto1.Clases;

public class Mensaje
{
      
 
private static string GetMensajeEnlace(Propietario perfil,string enlace="")
    {
       return $@"<p>Hola {perfil.Nombre}:</p>
              <p>Hemos recibido una solicitud de restablecimiento de contraseña de tu cuenta.</p>
              <p>Haz clic en el botón que aparece a continuación para cambiar tu contraseña.</p>
              <p>Ten en cuenta que este enlace es válido solo durante 24 horas. Una vez transcurrido el plazo, deberás volver a solicitar el restablecimiento de la contraseña.</p>
              <a href='{enlace}'>Cambiar contraseña</a>";  
    }
private static string GetMensajeCodigo(Propietario perfil,string codigo="")
    {
       return $@"<p>Hola {perfil.Nombre}:</p>
              <p>Hemos recibido una solicitud de restablecimiento de contraseña de tu cuenta.</p>
              <p>Haz clic en el botón que aparece a continuación para cambiar tu contraseña.</p>
              <p>Ten en cuenta que este enlace es válido solo durante 24 horas. Una vez transcurrido el plazo, deberás volver a solicitar el restablecimiento de la contraseña.</p>
              <h1>{codigo}</h1>";  
    }


public static async Task<bool>  EnviarEnlace(Propietario perfil,string subject, IConfiguration config,IWebHostEnvironment environment,JWT jwt, string dominio){
	
	      try{

			
			//	 var dominio = environment.IsDevelopment() ? config["AppSettings:DevelopmentDomain"] : config["AppSettings:ProductionDomain"];

				string token=new JwtSecurityTokenHandler().WriteToken( jwt.GenerarToken(perfil.Id,5));
                //string enlace=dominio+$"Propietario/token?access_token={token}";
				//string enlace = $"{dominio}/resetearpass.html?access_token={token}";
				  string enlace = $"http://192.168.199.91:5000/Propietario/resetearpass?access_token={token}";
				Console.WriteLine("enlace   tokem: "+enlace);


			
				 

              var message = new MimeKit.MimeMessage();
				message.To.Add(new MailboxAddress(perfil.Nombre, perfil.Email));
				message.From.Add(new MailboxAddress("Inmobiliaria TP", config["Email:SMTPUser"]));
				message.Subject = subject;
				message.Body = new TextPart("html") 
				{ Text=Mensaje.GetMensajeEnlace(perfil,enlace)
					,//falta enviar la clave generada (sin hashear)
				};
				
				MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,  
					System.Security.Cryptography.X509Certificates.X509Certificate certificate,
					System.Security.Cryptography.X509Certificates.X509Chain chain,
					System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
				{ return true; };
				client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(config["Email:SMTPUser"], config["Email:SMTPPass"]);//estas credenciales deben estar en el user secrets
				await client.SendAsync(message); 
				await client.DisconnectAsync(true);
				return true;
                }
				catch(Exception){
					return false;
				}
}





public static async Task<String>  EnviarCodigo(Propietario perfil,string subject, IConfiguration config,Password password){
	
	      try{
			    Random rand = new Random(Environment.TickCount);
				string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
				string nuevaClave = "";
				for (int i = 0; i < 8; i++)
				{
					nuevaClave += randomChars[rand.Next(0, randomChars.Length)];
				}//!Falta hacer el hash a la clave y actualizar el usuario con dicha clave
				

              var message = new MimeKit.MimeMessage();
				message.To.Add(new MailboxAddress(perfil.Nombre, perfil.Email));
				message.From.Add(new MailboxAddress("Inmobiliaria TP", config["Email:SMTPUser"]));
				message.Subject = subject;
				message.Body = new TextPart("html") // usar una vista
				{ Text=Mensaje.GetMensajeCodigo(perfil,nuevaClave)
					,//falta enviar la clave generada (sin hashear)
				};
				
				MailKit.Net.Smtp.SmtpClient client = new SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,  //para q no valide el certificado
					System.Security.Cryptography.X509Certificates.X509Certificate certificate,
					System.Security.Cryptography.X509Certificates.X509Chain chain,
					System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
				{ return true; };
				client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(config["Email:SMTPUser"], config["Email:SMTPPass"]);
				await client.SendAsync(message); 
				await client.DisconnectAsync(true);

				return password.HashPassword(nuevaClave);
                }
				catch(Exception){
					return "";
				}
}



}