package com.example.stackunderflow.ui.login

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import com.example.stackunderflow.MainActivity
import com.example.stackunderflow.R
import com.example.stackunderflowMobile.dto.LoginUserDto
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
        val registerButton: Button = findViewById(R.id.register_button)

        userViewModel.isLogged.observe(this, Observer { isLogged ->
            if (isLogged == true) {
                startActivity(Intent(this, MainActivity::class.java))
                finish()
            }
        })

        userViewModel.loginError.observe(this, Observer { error ->
            Toast.makeText(this, error, Toast.LENGTH_SHORT).show()
        })

        loginButton.setOnClickListener {
            val email = emailEditText.text.toString()
            val password = passwordEditText.text.toString()

            val loginUserDto = LoginUserDto(email,password)
            userViewModel.getLogin(loginUserDto)
            if(userViewModel.isLogged.value == false){
                Toast.makeText(this, "Invalid email or password", Toast.LENGTH_SHORT).show()
            }
        }

        registerButton.setOnClickListener {
            val intent = Intent(this, CreateUserActivity::class.java)
            startActivity(intent)
        }
    }
}

