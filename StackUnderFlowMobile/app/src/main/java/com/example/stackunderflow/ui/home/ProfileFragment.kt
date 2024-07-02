package com.example.stackunderflow.ui.home

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentProfileBinding
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
      userViewModel.getUserInfo()
    return root
  }

override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}