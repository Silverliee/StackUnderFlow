package com.example.stackunderflow.ui.login

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.EditText
import com.example.stackunderflow.MainActivity
import com.example.stackunderflow.R
import com.example.stackunderflow.dto.LoginUserDto
import com.example.stackunderflow.module.injectModuleDependencies
import com.example.stackunderflow.viewModels.UserViewModel
import androidx.lifecycle.Observer
import org.koin.androidx.viewmodel.ext.android.viewModel


class LoginActivity : AppCompatActivity() {

    private val userViewModel : UserViewModel by viewModel()


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)
        injectModuleDependencies(this@LoginActivity)

        val emailEditText: EditText = findViewById(R.id.username_input)
        val passwordEditText: EditText = findViewById(R.id.password_input)
        val loginButton: Button = findViewById(R.id.login_button)

        userViewModel.isLogged.observe(this, Observer { isLogged ->
            if (isLogged == true) {
                // L'utilisateur est connecté avec succès
                // Redirigez l'utilisateur vers une autre activité ou mettez à jour l'UI
                startActivity(Intent(this, MainActivity::class.java))
                finish()
            } else {
                // La connexion a échoué
                // Afficher un message d'erreur ou faire autre chose
                Log.d("LoginActivity", "Login failed")
            }
        })

        loginButton.setOnClickListener {
            val email = emailEditText.text.toString()
            val password = passwordEditText.text.toString()

            val loginUserDto = LoginUserDto(email,password)
            userViewModel.getLogin(loginUserDto)
        }
    }
}