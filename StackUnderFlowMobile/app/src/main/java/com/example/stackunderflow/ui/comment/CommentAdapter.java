package com.example.stackunderflow.ui.comment;

import android.app.AlertDialog;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.PopupMenu;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflow.dto.CommentDto;
import com.example.stackunderflow.dto.CommentRequestDto;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.List;

public class CommentAdapter extends RecyclerView.Adapter<CommentAdapter.ViewHolder> {

    private final Context context;
    private final List<CommentDto> commentDtos;
    private final CommentViewModel commentViewModel;

    // Constructor modification
    public CommentAdapter(Context context, List<CommentDto> comment, CommentViewModel commentViewModel) {
        this.context = context;
        this.commentDtos = comment;
        this.commentViewModel = commentViewModel;
    }

    @NonNull
    @Override
    public CommentAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.layout_list, parent, false);
        return new CommentAdapter.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull CommentAdapter.ViewHolder holder, int position) {
        CommentDto currentComment = commentDtos.get(position);
        holder.commentUsername.setText(currentComment.getUserName());
        holder.commentContent.setText(currentComment.getDescription());

        holder.commentSettings.setOnClickListener(v -> {
            popupMenus(v, holder.getAdapterPosition());
        });

        commentViewModel.getUser().observeForever(user -> {
            if(user.getUserId() == currentComment.getUserId()){
                holder.commentSettings.setVisibility(View.VISIBLE);
            }else {
                holder.commentSettings.setVisibility(View.GONE);
            }
        });
    }

    private void popupMenus(View v, int adapterPosition) {
        CommentDto currentComment = commentDtos.get(adapterPosition);
        PopupMenu popupMenu = new PopupMenu(context, v);
        popupMenu.inflate(R.menu.show_menu);

        popupMenu.setOnMenuItemClickListener(menuItem -> {
            if (menuItem.getItemId() == R.id.delete) {
                new AlertDialog.Builder(context)
                        .setTitle("Delete")
                        .setIcon(R.drawable.ic_warning)
                        .setMessage("Are you sure you want to delete this Information")
                        .setPositiveButton("Yes", (dialog, which) -> {
                            commentDtos.remove(adapterPosition);
                            commentViewModel.deleteComment(currentComment.getCommentId());
                            notifyDataSetChanged();
                            Toast.makeText(context, "Deleted this Information", Toast.LENGTH_SHORT).show();
                            dialog.dismiss();
                        })
                        .setNegativeButton("No", (dialog, which) -> dialog.dismiss())
                        .create()
                        .show();
                return true;
            } else if (menuItem.getItemId() == R.id.editText) {
                View view = LayoutInflater.from(context).inflate(R.layout.create_comment, null);
                EditText description = view.findViewById(R.id.newCommentContent);
                new AlertDialog.Builder(context)
                        .setView(view)
                        .setPositiveButton("Ok", (dialog, which) -> {
                            currentComment.setDescription(description.getText().toString());
                            commentViewModel.updateComment(currentComment.getCommentId(), new CommentRequestDto(description.getText().toString()));
                            notifyDataSetChanged();
                            Toast.makeText(context, "User Information is Edited", Toast.LENGTH_SHORT).show();
                            dialog.dismiss();
                        })
                        .setNegativeButton("Cancel", (dialog, which) -> dialog.dismiss())
                        .create()
                        .show();
                return true;
            }
            else {
                return false;
            }
        });

        popupMenu.show();
        try {
            Field popup = PopupMenu.class.getDeclaredField("mPopup");
            popup.setAccessible(true);
            Object menu = popup.get(popupMenu);
            Method method = menu.getClass().getDeclaredMethod("setForceShowIcon", boolean.class);
            method.setAccessible(true);
            method.invoke(menu, true);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    @Override
    public int getItemCount() {
        return commentDtos.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView commentUsername;
        TextView commentContent;
        Button commentSettings;


        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            commentUsername = itemView.findViewById(R.id.commentUserId);
            commentContent = itemView.findViewById(R.id.commentDescription);
            commentSettings = itemView.findViewById(R.id.mMenus);
        }
    }
}

