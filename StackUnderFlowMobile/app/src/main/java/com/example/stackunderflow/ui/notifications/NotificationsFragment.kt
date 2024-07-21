package com.example.stackunderflow.ui.notifications

import androidx.fragment.app.viewModels
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentNotificationsBinding
import com.example.stackunderflow.databinding.FragmentScriptBinding
import com.example.stackunderflow.repository.UsersRepository
import com.example.stackunderflow.ui.Scripts.RecyclerViewAdapterScripts
import com.example.stackunderflow.ui.Scripts.ScriptViewModel
import com.example.stackunderflow.ui.feed.FeedAdapter
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class NotificationsFragment : Fragment() {

    companion object {
        fun newInstance() = NotificationsFragment()
    }


    private var _binding: FragmentNotificationsBinding? = null
    private val viewModel: NotificationsViewModel by viewModel()
    private val userViewModel: UserViewModel by viewModel()
    private val binding get() = _binding!!


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentNotificationsBinding.inflate(inflater, container, false)
        val view = binding.root

        userViewModel.getFriendRequests()
        userViewModel.friendRequest.observe(viewLifecycleOwner) { friendRequest ->
            Log.d("ScriptFragment", "Received script: ${friendRequest.size}")
            val feedAdapter = NotificationsAdapter(userViewModel, context, friendRequest)
            binding.NotificationsRecyclerView.adapter = feedAdapter
            binding.NotificationsRecyclerView.layoutManager = LinearLayoutManager(context)
        }

        return view
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}