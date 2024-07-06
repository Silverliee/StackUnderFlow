package com.example.stackunderflow.ui.feed;


import android.annotation.SuppressLint;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflow.dto.ScriptModelDto;
import com.example.stackunderflow.ui.Scripts.RecyclerViewAdapterScripts;

import java.util.ArrayList;
import java.util.List;

public class FeedAdapter extends RecyclerView.Adapter<FeedAdapter.ViewHolder> {

        private final Context context;
        private final List<ScriptModelDto> scripts;

        // Constructor modification
        public FeedAdapter(Context context, List<ScriptModelDto> scripts) {
            this.context = context;
            this.scripts = scripts;
        }

        @NonNull
        @Override
        public FeedAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            LayoutInflater inflater = LayoutInflater.from(context);
            View view = inflater.inflate(R.layout.item_feed, parent, false);
            return new FeedAdapter.ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            holder.scriptName.setText(scripts.get(position).getScriptName());
            holder.scriptDescription.setText(scripts.get(position).getDescription());
            holder.scriptNumberOfLikes.setText(String.valueOf((scripts.get(position).getNumberOfLikes())));
            holder.scriptUsername.setText(scripts.get(position).getCreatorName());
        }

        @Override
        public int getItemCount() {
            return scripts.size();
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            TextView scriptUsername;
            TextView scriptName;
            TextView scriptDescription;
            TextView scriptNumberOfLikes;
            Button scriptCommentButton;
            Button scriptLikeButton;

            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                scriptUsername = itemView.findViewById(R.id.script_userIdFeed);
                scriptName = itemView.findViewById(R.id.script_nameFeed);
                scriptDescription = itemView.findViewById(R.id.script_descriptionFeed);
                scriptNumberOfLikes = itemView.findViewById(R.id.number_of_likesFeed);
                scriptCommentButton = itemView.findViewById(R.id.comment_buttonFeed);
            }
        }
    }