package com.example.stackunderflow.ui.feed

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.stackunderflow.databinding.FragmentFeedBinding
import com.example.stackunderflow.ui.Scripts.ScriptViewModel
import org.koin.androidx.viewmodel.ext.android.viewModel

class FeedFragment : Fragment() {

    private val feedViewModel: FeedViewModel by viewModel()
    private val scriptViewModel: ScriptViewModel by viewModel()
    private var _binding: FragmentFeedBinding? = null

    // This property is only valid between onCreateView and onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentFeedBinding.inflate(inflater, container, false)
        val view = binding.root

        scriptViewModel.GetMyScript()
        scriptViewModel.myScripts.observe(viewLifecycleOwner) { scripts ->
            Log.d("ScriptFragment", "Received script: ${scripts.size}")
            val feedAdapter = FeedAdapter(scriptViewModel ,context, scripts)
            binding.feedRecyclerView.adapter = feedAdapter
            binding.feedRecyclerView.layoutManager = LinearLayoutManager(context)
        }

        return view
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}