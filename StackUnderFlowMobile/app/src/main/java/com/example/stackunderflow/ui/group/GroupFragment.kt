package com.example.stackunderflow.ui.group


import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.databinding.FragmentGroupBinding
import com.example.stackunderflow.module.injectModuleDependencies
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel


class GroupFragment : Fragment() {

    companion object {
        private const val GROUP_ID = "group_id"

        @JvmStatic
        fun newInstance(groupId: Int): GroupFragment {
            val fragment = GroupFragment()
            val args = Bundle()
            args.putInt(GROUP_ID, groupId)
            fragment.arguments = args
            return fragment
        }
    }

    private var _binding: FragmentGroupBinding? = null
    private val userViewModel : UserViewModel by viewModel()
    private val binding get() = _binding!!
    private var groupID :Int = 0

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        injectModuleDependencies(this@GroupFragment.requireContext())
        _binding = FragmentGroupBinding.inflate(inflater, container, false)
        val view = binding.root
        userViewModel.getUserInfo()
        userViewModel.getGroupByUserId()
        Log.d("GroupFragment", "Received groupMembers: ${userViewModel.myGroup.value?.size}")
        groupID = arguments?.getInt(GROUP_ID, -1) ?: 0
        userViewModel.getGroupMembers(groupID)

        userViewModel.myGroup.observe(viewLifecycleOwner) { group ->
            group?.let {
                val groupe = group.find { it.groupId == groupID }
                if (groupe != null) {
                    userViewModel.getUserById(groupe.creatorUserID)
                }
                Log.d("GroupFragment", "Received groupMembers: ${groupe?.groupName}")
                binding.groupNameGroup.text = groupe?.groupName
                binding.groupDescriptionGroup.text = groupe?.description
                userViewModel.groupCreatorUser.observe(viewLifecycleOwner) { user ->
                    user?.let {
                        binding.creatorGroupName.text = user.username
                    }
                }
            }
        }

        userViewModel.groupMember.observe(viewLifecycleOwner) { members ->
            members?.let {
                Log.d("GroupFragment", "Received groupMembers: ${members.size}")
                val adapter = GroupAdapter(userViewModel, context, members, groupID)
                binding.commentRecyclerView.adapter = adapter
                binding.commentRecyclerView.layoutManager = LinearLayoutManager(context)
            } ?: run {
                Log.d("CommentFragment", "User data is null")
            }
        }

        binding.searchUserBtn.setOnClickListener {
            val searchText = binding.seachUsernameInput.text.toString()
            userViewModel.getUserByKeyword(searchText)

            userViewModel.users.observe(viewLifecycleOwner) { members ->
                members?.let {
                    Log.d("GroupFragment", "Received groupMembers: ${members.size}")
                    val adapter = GroupAdapter(userViewModel, context, members, groupID)
                    binding.commentRecyclerView.adapter = adapter
                    binding.commentRecyclerView.layoutManager = LinearLayoutManager(context)
                } ?: run {
                    Log.d("CommentFragment", "User data is null")
                }
            }
        }
        return view
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

}