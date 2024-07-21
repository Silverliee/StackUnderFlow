package com.example.stackunderflow.ui.notifications;

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
import com.example.stackunderflow.dto.FriendRequestDto;
import com.example.stackunderflow.viewModels.UserViewModel;

import java.util.List;

public class NotificationsAdapter extends RecyclerView.Adapter<NotificationsAdapter.ViewHolder> {

    private final Context context;
    private final List<FriendRequestDto> friendRequests;
    private final UserViewModel userViewModel;

    // Constructor modification
    public NotificationsAdapter(UserViewModel userViewModel, Context context, List<FriendRequestDto> friendRequests) {
        this.userViewModel = userViewModel;
        this.context = context;
        this.friendRequests = friendRequests;
    }

    @NonNull
    @Override
    public NotificationsAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.friend_request_item, parent, false);
        return new NotificationsAdapter.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull NotificationsAdapter.ViewHolder holder, int position) {
        FriendRequestDto friendRequestDto = friendRequests.get(position);
        holder.friendRequestUserName.setText(friendRequestDto.getFriendName() + " wants to be your friend");
        holder.friendRequestMessage.setText(friendRequestDto.getMessage());

        holder.acceptButton.setOnClickListener(v -> {
            userViewModel.acceptFriendRequest(friendRequestDto.getFriendId());
            friendRequests.remove(position);
            Toast.makeText(context, "Friend Request Accepted", Toast.LENGTH_SHORT).show();
            notifyDataSetChanged();
        });

        holder.declineButton.setOnClickListener(v -> {
            userViewModel.declineFriendRequest(friendRequestDto.getFriendId());
            friendRequests.remove(position);
            Toast.makeText(context, "Friend Request Declined", Toast.LENGTH_SHORT).show();
            notifyDataSetChanged();
        });
    }

    @Override
    public int getItemCount() {
        return friendRequests.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView friendRequestUserName;
        TextView friendRequestMessage;
        Button acceptButton;
        Button declineButton;

        boolean isLiked;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            friendRequestUserName = itemView.findViewById(R.id.friendRequestUserName);
            friendRequestMessage = itemView.findViewById(R.id.friendRequestMessage);
            acceptButton = itemView.findViewById(R.id.AcceptFriendRequest);
            declineButton = itemView.findViewById(R.id.DeclineFriendRequest);
        }
    }
}
