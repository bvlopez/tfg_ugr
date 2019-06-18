using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    string email;
    string password;

    public User(string newEmail, string newPassword) {
        email = newEmail;
        password = newPassword;
    }

    public string getEmail() {
        return email;
    }

    public string getPassword() {
        return password;
    }

     public void setEmail(string newEmail) {
        email = newEmail;
    }

    public void setPassword(string newPassword) {
        password = newPassword;
    }

}
