package com.example.stackunderflow.ui.group;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflow.models.User;
import com.example.stackunderflow.viewModels.UserViewModel;

import java.util.List;

public class GroupAdapter extends RecyclerView.Adapter<GroupAdapter.ViewHolder> {

    private final Context context;
    private final List<User> users;
    private final UserViewModel userViewModel;
    private final int groupID;

    // Constructor modification
    public GroupAdapter(UserViewModel userViewModel, Context context, List<User> users, int groupID) {
        this.context = context;
        this.users = users;
        this.userViewModel = userViewModel;
        this.groupID = groupID;
    }

    @NonNull
    @Override
    public GroupAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.members_group, parent, false);
        return new GroupAdapter.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull GroupAdapter.ViewHolder holder, int position) {
        User user = users.get(position);
        holder.usernameUser.setText(user.getUsername());

        if(userViewModel.getGroupMember().getValue().contains(user)){
            holder.addInGroup.setVisibility(View.GONE);
            holder.removeInGroup.setVisibility(View.VISIBLE);
        } else {
            holder.addInGroup.setVisibility(View.VISIBLE);
            holder.removeInGroup.setVisibility(View.GONE);
        }

        userViewModel.getGroupCreatorUser().observeForever(user1 -> {
            if (user1.getUserId() == user.getUserId()) {
                holder.addInGroup.setVisibility(View.GONE);
                holder.removeInGroup.setVisibility(View.GONE);
            }

        });

        holder.addInGroup.setOnClickListener(v -> {
            userViewModel.createGroupMember(groupID, user.getUserId());
            holder.addInGroup.setVisibility(View.GONE);
            Toast.makeText(context, "Request sent", Toast.LENGTH_SHORT).show();
        });

        holder.removeInGroup.setOnClickListener(v -> {
            userViewModel.deleteGroupMember(groupID, user.getUserId());
            holder.removeInGroup.setVisibility(View.GONE);
            users.remove(user);
            Toast.makeText(context, "User removed", Toast.LENGTH_SHORT).show();
        });
    }

    @Override
    public int getItemCount() {
        return users.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView usernameUser;
        Button addInGroup;
        Button removeInGroup;
        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            usernameUser = itemView.findViewById(R.id.groupUsername);
            addInGroup = itemView.findViewById(R.id.addInGroupButton);
            removeInGroup = itemView.findViewById(R.id.removeInGroupButton);
        }
    }

}

