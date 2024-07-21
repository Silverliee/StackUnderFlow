package com.example.stackunderflow.ui.slideshow

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.databinding.FragmentCommentBinding
import com.example.stackunderflow.databinding.FragmentFriendBinding
import com.example.stackunderflow.ui.Scripts.RecyclerViewAdapterScripts
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class FriendsFragment : Fragment() {

    private var _binding: FragmentFriendBinding? = null
    private val userViewModel : UserViewModel by viewModel()

    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        _binding = FragmentFriendBinding.inflate(inflater, container, false)
        val view = binding.root
        userViewModel.getMyFriends()
        userViewModel.users.observe(viewLifecycleOwner) { user ->
            Log.d("FriendsFragment", "Received user: ${user.size}")
            val friendAdapter = FriendAdapter(userViewModel, context, user)
            binding.searchUserRecyclerView.adapter = friendAdapter
            binding.searchUserRecyclerView.layoutManager = LinearLayoutManager(context)
        }

        binding.searchUserBtn.setOnClickListener {
            val searchText = binding.seachUsernameInput.text.toString()
            userViewModel.getUserByKeyword(searchText)
        }

        return view
    }

override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}