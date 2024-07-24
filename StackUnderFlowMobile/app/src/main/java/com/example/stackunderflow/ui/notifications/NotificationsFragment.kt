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
import java.util.Objects

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
        userViewModel.getGroupRequests()

        userViewModel.friendRequest.observe(viewLifecycleOwner) { friendRequests ->
            Log.d("ScriptFragment", "Received friend requests: ${friendRequests.size}")

            userViewModel.groupRequests.observe(viewLifecycleOwner) { groupRequests ->
                Log.d("ScriptFragment", "Received group requests: ${groupRequests.size}")

                val list = mutableListOf<Any>()
                list.addAll(friendRequests) // Ajoutez tous les éléments de friendRequests
                list.addAll(groupRequests) // Ajoutez tous les éléments de groupRequests

                val feedAdapter = NotificationsAdapter(userViewModel, requireContext(), list)
                binding.NotificationsRecyclerView.adapter = feedAdapter
                binding.NotificationsRecyclerView.layoutManager = LinearLayoutManager(requireContext())
            }
        }




        return view
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}