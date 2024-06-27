package com.example.stackunderflow.ui.gallery

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.example.stackunderflow.databinding.FragmentScriptBinding

class ScriptFragment : Fragment() {

private var _binding: FragmentScriptBinding? = null
  // This property is only valid between onCreateView and
  // onDestroyView.
  private val binding get() = _binding!!

  override fun onCreateView(
    inflater: LayoutInflater,
    container: ViewGroup?,
    savedInstanceState: Bundle?
  ): View {
    val scriptViewModel =
            ViewModelProvider(this).get(ScriptViewModel::class.java)

    _binding = FragmentScriptBinding.inflate(inflater, container, false)
    val root: View = binding.root

    /*val textView: TextView = binding.textGallery
    galleryViewModel.text.observe(viewLifecycleOwner) {
      textView.text = it
    }*/
    return root
  }

override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}