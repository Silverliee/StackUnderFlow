package com.example.stackunderflow.ui.login

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import com.example.stackunderflow.MainActivity
import com.example.stackunderflow.R
import com.example.stackunderflow.dto.UserModelDto
import com.example.stackunderflow.module.injectModuleDependencies
import com.example.stackunderflow.viewModels.UserViewModel
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

        this.userViewModel.userConnected.observe(this@LoginActivity){
                value ->
            val userconnected = value.username

            Log.e("USERNAME", userconnected)// print USERNAME
        }

        loginButton.setOnClickListener {
            val email = emailEditText.text.toString()
            val password = passwordEditText.text.toString()

            val userDto = UserModelDto(email,password)
            userViewModel.getLogin(userDto)




            if (email == "Test@gmail.com" && password == "test") {
                // Save login state
                getSharedPreferences("APP_PREFS", MODE_PRIVATE)
                    .edit()
                    .putBoolean("is_logged_in", true)
                    .apply()

                // Redirect to MainActivity
                val intent = Intent(this, MainActivity::class.java)
                startActivity(intent)
                finish()
            } else {
                Toast.makeText(this, "Invalid email or password", Toast.LENGTH_SHORT).show()
            }
        }
    }
}