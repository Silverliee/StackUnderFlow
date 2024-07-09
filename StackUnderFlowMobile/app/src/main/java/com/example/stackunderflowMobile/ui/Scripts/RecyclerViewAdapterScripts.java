package com.example.stackunderflowMobile.ui.Scripts;

import android.annotation.SuppressLint;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflowMobile.dto.ScriptModelDto;

import java.util.ArrayList;
import java.util.List;

public class RecyclerViewAdapterScripts extends RecyclerView.Adapter<RecyclerViewAdapterScripts.ViewHolder>{

    private final Context context;
    private final List<ScriptModelDto> scripts;
    private final List<ScriptModelDto> scriptsFull;

    // Constructor modification
    public RecyclerViewAdapterScripts(Context context, List<ScriptModelDto> scripts) {
        this.context = context;
        this.scripts = scripts;
        this.scriptsFull = new ArrayList<>(scripts); // Initialize the full list with a copy of the scripts
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
        holder.scriptName.setText(scripts.get(position).getScriptName());
        holder.scriptDescription.setText(scripts.get(position).getDescription());
        holder.scriptNumberOfLikes.setText(String.valueOf((scripts.get(position).getNumberOfLikes())));
    }

    @Override
    public int getItemCount() {
        return scripts.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView scriptName;
        TextView scriptDescription;
        TextView scriptNumberOfLikes;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            scriptName = itemView.findViewById(R.id.scriptNameTextView);
            scriptDescription = itemView.findViewById(R.id.descriptionTextView);
            scriptNumberOfLikes = itemView.findViewById(R.id.numberOfLikesTextView);

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
