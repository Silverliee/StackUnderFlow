package com.example.stackunderflow.ui.group

import android.app.AlertDialog
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.EditText
import android.widget.Toast
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentFeedBinding
import com.example.stackunderflow.databinding.FragmentMyGroupBinding
import com.example.stackunderflow.dto.CommentRequestDto
import com.example.stackunderflow.dto.GroupRequestDto
import com.example.stackunderflow.dto.GroupResponseDto
import com.example.stackunderflow.ui.Scripts.ScriptViewModel
import com.example.stackunderflow.ui.feed.FeedAdapter
import com.example.stackunderflow.ui.feed.FeedViewModel
import com.example.stackunderflow.viewModels.UserViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class MyGroupFragment : Fragment() {

    private val userViewModel: UserViewModel by viewModel()
    private var _binding: FragmentMyGroupBinding? = null

    // This property is only valid between onCreateView and onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentMyGroupBinding.inflate(inflater, container, false)
        val view = binding.root


        userViewModel.getGroupByUserId()
        userViewModel.myGroup.observe(viewLifecycleOwner) { groups ->
            Log.d("ScriptFragment", "Received script: ${groups.size}")
            val feedAdapter = MyGroupAdapter(userViewModel, context, groups)
            binding.searchGroupRecyclerView.adapter = feedAdapter
            binding.searchGroupRecyclerView.layoutManager = LinearLayoutManager(context)
        }


        binding.addingBtn.setOnClickListener {
            addInfo()
        }
        return view
    }

    private fun addInfo() {
        val inflater = LayoutInflater.from(this.context)
        val v = inflater.inflate(R.layout.create_group,null)

        val groupName = v.findViewById<EditText>(R.id.newGroupName)
        val groupDescription = v.findViewById<EditText>(R.id.newDescriptionGroup)

        val addDialog = AlertDialog.Builder(this.context)

        addDialog.setView(v)
        addDialog.setPositiveButton("Ok"){
                dialog,_->
              val newGroupName = groupName.text.toString()
              val newGroupDescription = groupDescription.text.toString()
              Log.d("GroupFragment", "GroupName: $newGroupName")
              val newGroup = GroupRequestDto(newGroupName,newGroupDescription)
             userViewModel.createGroup(newGroup)

            Toast.makeText(this.context,"Creation of the new Group Success", Toast.LENGTH_SHORT).show()
            dialog.dismiss()
        }
        addDialog.setNegativeButton("Cancel"){
                dialog,_->
            dialog.dismiss()
            Toast.makeText(this.context,"Cancel", Toast.LENGTH_SHORT).show()

        }
        addDialog.create()
        addDialog.show()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}