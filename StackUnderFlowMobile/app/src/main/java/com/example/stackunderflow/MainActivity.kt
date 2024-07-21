package com.example.stackunderflow

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.widget.Button
import android.widget.TextView
import com.google.android.material.navigation.NavigationView
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import androidx.drawerlayout.widget.DrawerLayout
import androidx.appcompat.app.AppCompatActivity
import androidx.lifecycle.Observer
import com.example.stackunderflow.databinding.ActivityMainBinding
import com.example.stackunderflow.module.injectModuleDependencies
import com.example.stackunderflow.ui.Scripts.ScriptViewModel
import com.example.stackunderflow.ui.login.LoginActivity
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class MainActivity : AppCompatActivity() {

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding
    private val userViewModel : UserViewModel by viewModel()
    private val scriptViewModel : ScriptViewModel by viewModel()


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        injectModuleDependencies(this@MainActivity)
        // Check if user is logged in
        userViewModel.getUserInfo()
        scriptViewModel.GetMyScript()
        val sharedPreferences = getSharedPreferences("APP_PREFS", MODE_PRIVATE)
        val isLoggedIn = sharedPreferences.getBoolean("is_logged_in", false)
        if (!isLoggedIn) {
            // Redirect to LoginActivity
            val intent = Intent(this, LoginActivity::class.java)
            startActivity(intent)
            finish()
            return
        }

        userViewModel.user.observe(this) { user ->
            if (user != null) {
                // User data is loaded, you can use it here or notify other components
                Log.d("MainActivity", "User data loaded: $user")
            } else {
                Log.d("MainActivity", "User data is null")
            }
        }

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.appBarMain.toolbar)

        val drawerLayout: DrawerLayout = binding.drawerLayout
        val navView: NavigationView = binding.navView
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.nav_profile, R.id.nav_scripts, R.id.nav_friends, R.id.nav_feed
            ), drawerLayout
        )
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)


    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.main, menu)
        return true
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)

        val logoutButton: Button = findViewById(R.id.button_log_out)
        val UsernameTexteView: TextView = findViewById(R.id.UserProfileNameNavBar)
        // Set a click listener for the logout button
        logoutButton.setOnClickListener {
            userViewModel.isLogged.value = false
            val intent = Intent(this, LoginActivity::class.java)
            startActivity(intent)
            finish()
        }

        userViewModel.user.observe(this, Observer { user ->
            UsernameTexteView.text = user.username
        })

        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }
}