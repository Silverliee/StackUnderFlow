package com.example.stackunderflow.ui.Scripts

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.databinding.FragmentScriptBinding
import org.koin.androidx.viewmodel.ext.android.viewModel

class ScriptFragment : Fragment() {

    private val scriptViewModel: ScriptViewModel by viewModel()
    private var _binding: FragmentScriptBinding? = null
      // This property is only valid between onCreateView and
      // onDestroyView.
      private val binding get() = _binding!!


    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentScriptBinding.inflate(inflater, container, false)
        val view = binding.root

        scriptViewModel.GetMyScript()
        scriptViewModel.myScripts.observe(viewLifecycleOwner) { scripts ->
            Log.d("ScriptFragment", "Received script: ${scripts.size}")
            val adapterScripts = RecyclerViewAdapterScripts(context, scripts)
            binding.searchScriptRecyclerView.adapter = adapterScripts
            binding.searchScriptRecyclerView.layoutManager = LinearLayoutManager(context)
        }

        binding.searchUserBtn.setOnClickListener {
            val searchText = binding.seachScriptInput.text.toString()
            val adapterScripts = binding.searchScriptRecyclerView.adapter as RecyclerViewAdapterScripts
            adapterScripts.filter(searchText)
        }

        return view
    }

    override fun onDestroyView() {
            super.onDestroyView()
            _binding = null
        }
}