package com.example.stackunderflow.ui.notifications;

import android.content.Context;
import android.util.Log;
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
import com.example.stackunderflow.dto.GroupRequestResponseDto;
import com.example.stackunderflow.viewModels.UserViewModel;

import java.util.List;

public class NotificationsAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {
    private static final int VIEW_TYPE_FRIEND_REQUEST = 0;
    private static final int VIEW_TYPE_GROUP_REQUEST = 1;

    private final Context context;
    private final List<Object> notifications; // Utiliser Object pour stocker différents types d'éléments
    private final UserViewModel userViewModel;

    public NotificationsAdapter(UserViewModel userViewModel, Context context, List<Object> notifications) {
        this.userViewModel = userViewModel;
        this.context = context;
        this.notifications = notifications;
    }

    @Override
    public int getItemViewType(int position) {
        Object item = notifications.get(position);
        Log.d("NotificationsAdapter", "Item at position " + position + " is of type: " + item.getClass().getSimpleName());
        if (item instanceof FriendRequestDto) {
            return VIEW_TYPE_FRIEND_REQUEST;
        } else if (item instanceof GroupRequestResponseDto) {
            return VIEW_TYPE_GROUP_REQUEST;
        }
        return -1;
    }

    @NonNull
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(context);
        if (viewType == VIEW_TYPE_FRIEND_REQUEST) {
            View view = inflater.inflate(R.layout.friend_request_item, parent, false);
            return new FriendRequestViewHolder(view);
        } else if (viewType == VIEW_TYPE_GROUP_REQUEST) {
            View view = inflater.inflate(R.layout.group_request_item, parent, false);
            return new GroupRequestViewHolder(view);
        }
        throw new IllegalArgumentException("Invalid view type");
    }

    @Override
    public void onBindViewHolder(@NonNull RecyclerView.ViewHolder holder, int position) {
        int viewType = getItemViewType(position);
        if (viewType == VIEW_TYPE_FRIEND_REQUEST) {
            ((FriendRequestViewHolder) holder).bind((FriendRequestDto) notifications.get(position));
        } else if (viewType == VIEW_TYPE_GROUP_REQUEST) {
            ((GroupRequestViewHolder) holder).bind((GroupRequestResponseDto) notifications.get(position));
        }
    }

    @Override
    public int getItemCount() {
        return notifications.size();
    }

    public class FriendRequestViewHolder extends RecyclerView.ViewHolder {
        TextView friendRequestUserName;
        TextView friendRequestMessage;
        Button acceptButton;
        Button declineButton;

        public FriendRequestViewHolder(@NonNull View itemView) {
            super(itemView);
            friendRequestUserName = itemView.findViewById(R.id.friendRequestUserName);
            friendRequestMessage = itemView.findViewById(R.id.friendRequestMessage);
            acceptButton = itemView.findViewById(R.id.AcceptFriendRequest);
            declineButton = itemView.findViewById(R.id.DeclineFriendRequest);
        }

        public void bind(FriendRequestDto friendRequestDto) {
            friendRequestUserName.setText(friendRequestDto.getFriendName() + " wants to be your friend");
            friendRequestMessage.setText(friendRequestDto.getMessage());

            acceptButton.setOnClickListener(v -> {
                userViewModel.acceptFriendRequest(friendRequestDto.getFriendId());
                notifications.remove(getAdapterPosition());
                Toast.makeText(context, "Friend Request Accepted", Toast.LENGTH_SHORT).show();
                notifyDataSetChanged();
            });

            declineButton.setOnClickListener(v -> {
                userViewModel.declineFriendRequest(friendRequestDto.getFriendId());
                notifications.remove(getAdapterPosition());
                Toast.makeText(context, "Friend Request Declined", Toast.LENGTH_SHORT).show();
                notifyDataSetChanged();
            });
        }
    }

    public class GroupRequestViewHolder extends RecyclerView.ViewHolder {
        TextView groupRequestName;
        TextView groupRequestMessage;
        Button acceptButton;
        Button declineButton;

        public GroupRequestViewHolder(@NonNull View itemView) {
            super(itemView);
            groupRequestName = itemView.findViewById(R.id.groupRequestName);
            groupRequestMessage = itemView.findViewById(R.id.group_descriptionGroup);
            acceptButton = itemView.findViewById(R.id.AcceptGroupRequest);
            declineButton = itemView.findViewById(R.id.DeclineGroupRequest);
        }

        public void bind(GroupRequestResponseDto groupRequestDto) {
            groupRequestName.setText(groupRequestDto.getGroupName() + " wants you to join");

            acceptButton.setOnClickListener(v -> {
                userViewModel.acceptGroupRequest(groupRequestDto.getGroupId());
                notifications.remove(getAdapterPosition());
                Toast.makeText(context, "Group Request Accepted", Toast.LENGTH_SHORT).show();
                notifyDataSetChanged();
            });

            declineButton.setOnClickListener(v -> {
                userViewModel.declineGroupRequest(groupRequestDto.getGroupId());
                notifications.remove(getAdapterPosition());
                Toast.makeText(context, "Group Request Declined", Toast.LENGTH_SHORT).show();
                notifyDataSetChanged();
            });
        }
    }
}
