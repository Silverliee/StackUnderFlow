package com.example.stackunderflow.ui.Scripts

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.R
import com.example.stackunderflow.databinding.FragmentScriptBinding
import com.example.stackunderflow.databinding.FragmentScriptInfoBinding
import com.example.stackunderflow.ui.comment.CommentFragment
import com.example.stackunderflow.ui.comment.CommentFragment.Companion
import org.koin.androidx.viewmodel.ext.android.viewModel

class ScriptInfoFragment : Fragment() {

    companion object {
        private const val SCRIPT_ID = "script_id"

        @JvmStatic
        fun newInstance(scriptId: Int): ScriptInfoFragment {
            val fragment = ScriptInfoFragment()
            val args = Bundle()
            args.putInt(SCRIPT_ID, scriptId)
            fragment.arguments = args
            return fragment
        }
    }

    private val scriptViewModel: ScriptViewModel by viewModel()
    private var _binding: FragmentScriptInfoBinding? = null
    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!
    private var scriptId : Int = -1


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentScriptInfoBinding.inflate(inflater, container, false)
        val view = binding.root

        scriptViewModel.GetMyScript()
        scriptId = arguments?.getInt(SCRIPT_ID, -1) ?: 0

        Log.d("CommentFragment", "Script id: $scriptId")
        Log.d("CommentFragment", "Script id: ${scriptViewModel.myScripts.value?.size}")
        scriptViewModel.myScripts.observe(viewLifecycleOwner) { scripts ->
            scripts?.let {
                val script = scripts.find { it.scriptId == scriptId }
                binding.scriptName.text = script?.scriptName
                binding.scriptDescription.text = script?.description
                binding.scriptUserId.text = script?.creatorName
                binding.scriptCreationDate.text = script?.creationDate
                binding.scriptInputType.text = script?.inputScriptType
                binding.scriptOutputType.text = script?.outputScriptType
                binding.numberOfLikes.text = script?.numberOfLikes.toString()
                binding.scriptProgrammingLanguage.text = script?.programmingLanguage
            }
        }

        binding.returnButton.setOnClickListener {
            parentFragmentManager.beginTransaction()
                .remove(this@ScriptInfoFragment)
                .commit()
        }

        return view
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}