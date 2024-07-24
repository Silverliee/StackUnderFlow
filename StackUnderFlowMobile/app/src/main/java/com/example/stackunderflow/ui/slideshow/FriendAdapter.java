package com.example.stackunderflow.ui.slideshow;

import android.app.AlertDialog;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflow.dto.FriendRequestCreationRequestDto;
import com.example.stackunderflow.models.User;
import com.example.stackunderflow.viewModels.UserViewModel;

import java.util.List;

public class FriendAdapter extends RecyclerView.Adapter<FriendAdapter.ViewHolder> {

    private final Context context;
    private final List<User> users;
    private final UserViewModel userViewModel;

    // Constructor modification
    public FriendAdapter(UserViewModel userViewModel, Context context, List<User> Users) {
        this.userViewModel = userViewModel;
        this.context = context;
        this.users = Users;
    }

    @NonNull
    @Override
    public FriendAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.item_friends, parent, false);
        return new FriendAdapter.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull FriendAdapter.ViewHolder holder, int position) {
        User user = users.get(position);
        holder.friendName.setText(user.getUsername());


        if(userViewModel.getFriend().getValue().contains(user)){
            holder.addFriend.setVisibility(View.GONE);
            holder.removeFriend.setVisibility(View.VISIBLE);
        } else {
            holder.addFriend.setVisibility(View.VISIBLE);
            holder.removeFriend.setVisibility(View.GONE);
        }

        holder.addFriend.setOnClickListener(v -> {
            addInfo(holder,user);
            holder.addFriend.setVisibility(View.GONE);
        });

        holder.removeFriend.setOnClickListener(v -> {
            userViewModel.deleteFriend(user.getUserId());
            holder.addFriend.setVisibility(View.VISIBLE);
            holder.removeFriend.setVisibility(View.GONE);
            userViewModel.getFriend().getValue().remove(user);
        });
    }


    private void addInfo(FriendAdapter.ViewHolder holder, User user) {
        LayoutInflater inflater = LayoutInflater.from(context);
        View v = inflater.inflate(R.layout.create_friend_request, null);

        /**set view*/
        EditText commentDescription = v.findViewById(R.id.newMessageFriendRequest);

        AlertDialog.Builder addDialog = new AlertDialog.Builder(context);

        addDialog.setView(v);
        addDialog.setPositiveButton("Ok", (dialog, which) -> {
            String message = commentDescription.getText().toString();
            Log.d("CommentFragment", "Comment: " + message);
            FriendRequestCreationRequestDto newFriendRequest = new FriendRequestCreationRequestDto(message);
            userViewModel.createFriendRequest(user.getUserId() ,newFriendRequest);
            Toast.makeText(context, "Friend Request Send", Toast.LENGTH_SHORT).show();
            holder.addFriend.setVisibility(View.GONE);
            dialog.dismiss();
        });

        addDialog.setNegativeButton("Cancel", (dialog, which) -> {
            dialog.dismiss();
            Toast.makeText(context, "Cancel", Toast.LENGTH_SHORT).show();
        });

        addDialog.create();
        addDialog.show();
    }




    @Override
    public int getItemCount() {
        return users.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView friendName;
        Button addFriend;
        Button removeFriend;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            friendName = itemView.findViewById(R.id.UsernameFriendResult);
            addFriend = itemView.findViewById(R.id.addFriendRequest);
            removeFriend = itemView.findViewById(R.id.removeFriend);
        }
    }

}