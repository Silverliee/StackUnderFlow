package com.example.stackunderflow.ui.login

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import androidx.appcompat.app.AppCompatActivity
import com.example.stackunderflow.R
import com.example.stackunderflow.dto.RegisterUserDto
import com.example.stackunderflow.viewModels.UserViewModel
import androidx.lifecycle.Observer
import org.koin.androidx.viewmodel.ext.android.viewModel

class CreateUserActivity : AppCompatActivity() {

    private val userViewModel : UserViewModel by viewModel()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.create_account)

        val usernameEditText: EditText = findViewById(R.id.username_input)
        val emailEditText: EditText = findViewById(R.id.email_input)
        val passwordEditText: EditText = findViewById(R.id.password_input)
        val createAccountButton: Button = findViewById(R.id.create_account_button)

        userViewModel.isRegistered.observe(this, Observer { isRegistered ->
            if (isRegistered) {
                val intent = Intent(this, LoginActivity::class.java)
                startActivity(intent)
                finish()
            }
        })

        createAccountButton.setOnClickListener {
            val userName = usernameEditText.text.toString()
            val email = emailEditText.text.toString()
            val password = passwordEditText.text.toString()

            val registerUserDto = RegisterUserDto(userName, email, password)
            userViewModel.getRegister(registerUserDto)
        }
    }
}