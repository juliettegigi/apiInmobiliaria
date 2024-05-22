using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace proyecto1.Clases;

public class JWT
{
 private string secretKey;
 private string issuer;
 private string audience;

 public JWT(string secretKey,string issuer,string audience){
         this.secretKey=secretKey;
         this.issuer=issuer;
         this.audience=audience;
 }


public JwtSecurityToken  GenerarToken(int id, int minutos){
   var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secretKey));
   var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
   var claims = new List<Claim>{
   	    new Claim(ClaimTypes.Name, id.ToString()),
   	    new Claim(ClaimTypes.Role, "Propietario")
    };
   
   var token = new JwtSecurityToken(
   	    issuer: this.issuer,
   	    audience: audience,
   	    claims: claims,
   	    expires: DateTime.Now.AddMinutes(minutos),
   	    signingCredentials: credenciales
   );

   return token; 

}
}