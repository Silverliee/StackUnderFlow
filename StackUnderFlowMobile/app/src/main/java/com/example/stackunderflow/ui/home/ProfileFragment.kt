package com.example.stackunderflow.ui.home

import android.app.AlertDialog
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.LiveData
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentProfileBinding
import com.example.stackunderflow.dto.CommentRequestDto
import com.example.stackunderflow.dto.PasswordDto
import com.example.stackunderflow.dto.RegisterUserDto
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class ProfileFragment : Fragment() {

private var _binding: FragmentProfileBinding? = null
    private val userViewModel : UserViewModel by viewModel()
  // This property is only valid between onCreateView and
  // onDestroyView.
  private val binding get() = _binding!!

  override fun onCreateView(
    inflater: LayoutInflater,
    container: ViewGroup?,
    savedInstanceState: Bundle?
  ): View {
    val profileViewModel =
            ViewModelProvider(this).get(ProfileViewModel::class.java)

    _binding = FragmentProfileBinding.inflate(inflater, container, false)
    val root: View = binding.root
      // Get the TextViews
      val usernameTopTextView: TextView = root.findViewById(R.id.textViewProfileName)
      val emailTopTextView: TextView = root.findViewById(R.id.textViewEmailTop)
      val usernameTextView: TextView = root.findViewById(R.id.textViewUsernameChange)
      val emailTextView: TextView = root.findViewById(R.id.textViewEmailChange)
      //val passwordTextView: TextView = root.findViewById(R.id.textViewPasswordChange)

      // Observe the user LiveData
      userViewModel.user.observe(viewLifecycleOwner, Observer { user ->
          usernameTopTextView.text = user.username
          usernameTextView.text = user.username
          emailTextView.text = user.email
          emailTopTextView.text = user.email
      })

      binding.textViewEmailChange.setOnClickListener {
          addInfo(0)
      }

      binding.textViewUsernameChange.setOnClickListener {
          addInfo(1)
      }
      userViewModel.getUserInfo()
    return root
  }

    private var isChecking = false

    private var hasShownPasswordDialog = false

    private fun addInfo(changeInt: Int) {
        val inflater = LayoutInflater.from(this.context)
        val v = inflater.inflate(R.layout.user_info, null)
        /** set view */
        val commentDescription = v.findViewById<EditText>(R.id.user_info_input)

        val addDialog = AlertDialog.Builder(this.context)

        addDialog.setView(v)
        addDialog.setPositiveButton("Ok") { dialog, _ ->
            if (isChecking) return@setPositiveButton

            isChecking = true
            hasShownPasswordDialog = false

            when (changeInt) {
                0 -> {
                    userViewModel.checkEmailAvailability(commentDescription.text.toString())
                    val emailObserver = Observer<Boolean> { isAvailable ->
                        if (isAvailable) {
                            Toast.makeText(this.context, "Email already exists", Toast.LENGTH_SHORT).show()
                            isChecking = false
                        } else if (!hasShownPasswordDialog) {
                            hasShownPasswordDialog = true
                            showPasswordDialog(changeInt, commentDescription.text.toString())
                            dialog.dismiss()
                        }
                    }
                    userViewModel.isAvailable.observe(viewLifecycleOwner, emailObserver)
                }
                1 -> {
                    userViewModel.checkUsernameAvailability(commentDescription.text.toString())
                    val usernameObserver = Observer<Boolean> { isAvailable ->
                        if (isAvailable) {
                            Toast.makeText(this.context, "Username already exists", Toast.LENGTH_SHORT).show()
                            isChecking = false
                        } else if (!hasShownPasswordDialog) {
                            hasShownPasswordDialog = true
                            showPasswordDialog(changeInt, commentDescription.text.toString())
                            dialog.dismiss()
                        }
                    }
                    userViewModel.isAvailable.observe(viewLifecycleOwner, usernameObserver)
                }
            }
        }
        addDialog.setNegativeButton("Cancel") { dialog, _ ->
            dialog.dismiss()
            Toast.makeText(this.context, "Cancel", Toast.LENGTH_SHORT).show()
        }
        addDialog.create()
        addDialog.show()
    }

    private fun showPasswordDialog(changeInt: Int, newValue: String) {
        val inflater = LayoutInflater.from(this.context)
        val v = inflater.inflate(R.layout.password_layout, null)
        val passwordInput = v.findViewById<EditText>(R.id.enterPassword)

        val passwordDialog = AlertDialog.Builder(this.context)
        passwordDialog.setView(v)
        passwordDialog.setPositiveButton("Confirm") { dialog, _ ->
            val password = passwordInput.text.toString()
            if (password.isNotEmpty()) {
                verifyPasswordAndUpdateInfo(changeInt, newValue, password)
                dialog.dismiss()
            } else {
                Toast.makeText(this.context, "Please enter your password", Toast.LENGTH_SHORT).show()
            }
        }
        passwordDialog.setNegativeButton("Cancel") { dialog, _ ->
            dialog.dismiss()
            isChecking = false
            Toast.makeText(this.context, "Cancel", Toast.LENGTH_SHORT).show()
        }
        passwordDialog.create()
        passwordDialog.show()
    }

    private fun verifyPasswordAndUpdateInfo(changeInt: Int, newValue: String, password: String) {
        val passwordAvailability = PasswordDto(password)
        userViewModel.checkPasswordValidity(passwordAvailability)

        userViewModel.isCorrect.observe(viewLifecycleOwner, Observer { isCorrect ->
            if (isCorrect) {
                val updateUser = when (changeInt) {
                    0 -> RegisterUserDto(
                        email = newValue,
                        userName = userViewModel.user.value!!.username,
                        password = passwordAvailability.password
                    )
                    1 -> RegisterUserDto(
                        email = userViewModel.user.value!!.email,
                        userName = newValue,
                        password = passwordAvailability.password
                    )
                    else -> null
                }
                updateUser?.let {
                    userViewModel.updateUserInfo(it)
                    Toast.makeText(this.context, "Change User Information Success", Toast.LENGTH_SHORT).show()
                }
            } else {
//                Toast.makeText(this.context, "Incorrect password", Toast.LENGTH_SHORT).show()
            }
            isChecking = false
        })
    }


    fun <T> LiveData<T>.observeOnce(lifecycleOwner: LifecycleOwner, observer: (T) -> Unit) {
        val wrappedObserver = object : Observer<T> {
            override fun onChanged(t: T) {
                observer(t)
                removeObserver(this)
            }
        }
        observe(lifecycleOwner, wrappedObserver)
    }




    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}