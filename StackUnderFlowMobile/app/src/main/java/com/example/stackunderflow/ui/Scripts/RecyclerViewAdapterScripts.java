package com.example.stackunderflow.ui.Scripts;

import android.annotation.SuppressLint;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.fragment.app.FragmentActivity;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflow.dto.ScriptModelDto;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

public class RecyclerViewAdapterScripts extends RecyclerView.Adapter<RecyclerViewAdapterScripts.ViewHolder>{

    private final Context context;
    private final List<ScriptModelDto> scripts;
    private final List<ScriptModelDto> scriptsFull;
    private final ScriptViewModel scriptViewModel;

    // Constructor modification
    public RecyclerViewAdapterScripts(Context context, List<ScriptModelDto> scripts, ScriptViewModel scriptViewModel) {
        this.context = context;
        this.scripts = scripts;
        this.scriptsFull = new ArrayList<>(scripts); // Initialize the full list with a copy of the scripts
        this.scriptViewModel = scriptViewModel;
    }

    @NonNull
    @Override
    public RecyclerViewAdapterScripts.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.item_scritps, parent, false);
        return new RecyclerViewAdapterScripts.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull RecyclerViewAdapterScripts.ViewHolder holder, int position) {
        ScriptModelDto currentScript = scripts.get(position);
        holder.scriptName.setText(currentScript.getScriptName());
        holder.scriptDescription.setText(currentScript.getDescription());

        if(Objects.equals(currentScript.getProgrammingLanguage(), "Csharp")){
            holder.scriptImage.setImageResource(R.drawable.doc_c_black);
        } else if (Objects.equals(currentScript.getProgrammingLanguage(), "Python")) {
            holder.scriptImage.setImageResource(R.drawable.doc_python_black);
        }else {
            holder.scriptImage.setImageResource(R.drawable.file_script);
        }

        if (currentScript.isFavorite()) {
            holder.scriptFavoriteButton.setImageResource(R.drawable.baseline_bookmark_added_24);
        } else {
            holder.scriptFavoriteButton.setVisibility(View.GONE);
        }

        holder.moreInfoButton.setOnClickListener(view -> {
            if (context instanceof FragmentActivity) {
                FragmentActivity fragmentActivity = (FragmentActivity) context;
                ScriptInfoFragment scriptInfoFragment = ScriptInfoFragment.newInstance(currentScript.getScriptId());
                fragmentActivity.getSupportFragmentManager().beginTransaction()
                        .replace(R.id.fragment_container, scriptInfoFragment) // Remplace le fragment actuel
                        .addToBackStack(null) // Permet de revenir en arrière pour fermer ce fragment
                        .commit();
            }
        });

        holder.scriptFavoriteButton.setOnClickListener(view -> {
            if (currentScript.isFavorite()) {
                List<ScriptModelDto> scripts = scriptViewModel.getMyScripts().getValue();
                if (scripts != null) {
                    int newposition = scripts.indexOf(currentScript);
                    if (newposition >= 0 && newposition < scripts.size()) {
                        scripts.remove(newposition);
                        scriptViewModel.deleteFavorite(currentScript.getScriptId());
                        notifyItemRemoved(newposition);
                    }
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return scripts.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView scriptName;
        TextView scriptDescription;
        ImageView scriptImage;
        ImageButton scriptFavoriteButton;
        ImageButton moreInfoButton;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            scriptImage = itemView.findViewById(R.id.imageView_script);
            scriptName = itemView.findViewById(R.id.scriptNameTextView);
            scriptDescription = itemView.findViewById(R.id.descriptionTextView);
            scriptFavoriteButton = itemView.findViewById(R.id.imageViewFavorite);
            moreInfoButton = itemView.findViewById(R.id.moreInfoButton);
        }
    }

    @SuppressLint("NotifyDataSetChanged")
    public void filter(String text) {
        List<ScriptModelDto> filteredList = new ArrayList<>();
        if(text.isEmpty()){
            filteredList.addAll(scriptsFull);
        } else{
            text = text.toLowerCase();
            for(ScriptModelDto item: scriptsFull){
                if(item.getScriptName().toLowerCase().contains(text)){
                    filteredList.add(item);
                }
            }
        }
        scripts.clear();
        scripts.addAll(filteredList);
        notifyDataSetChanged();
    }
}
