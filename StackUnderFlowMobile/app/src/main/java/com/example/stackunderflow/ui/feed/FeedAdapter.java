package com.example.stackunderflow.ui.feed;


import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.fragment.app.FragmentActivity;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflow.dto.ScriptModelDto;
import com.example.stackunderflow.ui.Scripts.ScriptViewModel;
import com.example.stackunderflow.ui.comment.CommentFragment;

import java.util.List;

public class FeedAdapter extends RecyclerView.Adapter<FeedAdapter.ViewHolder> {

    private final Context context;
    private final List<ScriptModelDto> scripts;
    private final ScriptViewModel scriptViewModel;

    // Constructor modification
    public FeedAdapter(ScriptViewModel scriptViewModel, Context context, List<ScriptModelDto> scripts) {
        this.scriptViewModel = scriptViewModel;
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
        ScriptModelDto currentScript = scripts.get(position);
        holder.scriptName.setText(currentScript.getScriptName());
        holder.scriptDescription.setText(currentScript.getDescription());
        holder.scriptNumberOfLikes.setText(String.valueOf(currentScript.getNumberOfLikes()));
        holder.scriptUsername.setText(currentScript.getCreatorName());

        if (currentScript.isLiked()) {
            holder.scriptLikeButton.setImageResource(R.drawable.baseline_favorite_24_red); // ic_like_red est l'ID de la ressource pour l'icône rouge
        } else {
            holder.scriptLikeButton.setImageResource(R.drawable.baseline_favorite_24); // Utilisez l'icône par défaut ou une autre si nécessaire
        }

        if (currentScript.isFavorite()) {
            holder.scriptFavoriteButton.setImageResource(R.drawable.baseline_bookmark_added_24);
        } else {
            holder.scriptFavoriteButton.setImageResource(R.drawable.baseline_bookmark_add_24);
        }

        holder.scriptLikeButton.setOnClickListener(view -> {
            if (currentScript.isLiked()) {
                scriptViewModel.DeleteLike(currentScript.getScriptId());
                currentScript.setLiked(false);
                currentScript.setNumberOfLikes(currentScript.getNumberOfLikes() - 1);
                holder.scriptLikeButton.setImageResource(R.drawable.baseline_favorite_24);

            } else {
                scriptViewModel.CreateLike(currentScript.getScriptId());
                currentScript.setLiked(true);
                currentScript.setNumberOfLikes(currentScript.getNumberOfLikes() + 1);
                holder.scriptLikeButton.setImageResource(R.drawable.baseline_favorite_24_red);

            }
            holder.scriptNumberOfLikes.setText(String.valueOf(currentScript.getNumberOfLikes()));
        });

        holder.scriptFavoriteButton.setOnClickListener(view -> {
            if (currentScript.isFavorite()) {
                //scriptViewModel.DeleteFavorite(currentScript.getScriptId());
                currentScript.setFavorite(false);
                holder.scriptFavoriteButton.setImageResource(R.drawable.baseline_bookmark_add_24);
            } else {
                scriptViewModel.CreateFavorite(currentScript.getScriptId());
                currentScript.setFavorite(true);
                holder.scriptFavoriteButton.setImageResource(R.drawable.baseline_bookmark_added_24);

            }
        });

        holder.scriptCommentButton.setOnClickListener(view -> {
            if (context instanceof FragmentActivity) {
                FragmentActivity fragmentActivity = (FragmentActivity) context;
                CommentFragment commentFragment = CommentFragment.newInstance(currentScript.getScriptId());
                fragmentActivity.getSupportFragmentManager().beginTransaction()
                        .replace(R.id.fragment_container, commentFragment) // Remplace le fragment actuel
                        .addToBackStack(null) // Permet de revenir en arrière pour fermer ce fragment
                        .commit();
            }
        });



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
        ImageButton scriptCommentButton;
        ImageButton scriptLikeButton;
        ImageButton scriptFavoriteButton;

        boolean isLiked;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            scriptUsername = itemView.findViewById(R.id.script_userIdFeed);
            scriptName = itemView.findViewById(R.id.script_nameFeed);
            scriptDescription = itemView.findViewById(R.id.script_descriptionFeed);
            scriptNumberOfLikes = itemView.findViewById(R.id.number_of_likesFeed);
            scriptCommentButton = itemView.findViewById(R.id.CommentButtonFeed);
            scriptLikeButton = itemView.findViewById(R.id.likeButtonFeed);
            scriptFavoriteButton = itemView.findViewById(R.id.favoriteButtonFeed);
            isLiked = false;
        }
    }
}