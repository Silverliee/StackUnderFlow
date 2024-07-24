package com.example.stackunderflow.ui.group;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.fragment.app.FragmentActivity;
import androidx.recyclerview.widget.RecyclerView;

import com.example.stackunderflow.R;
import com.example.stackunderflow.dto.GroupResponseDto;
import com.example.stackunderflow.viewModels.UserViewModel;

import java.util.List;

public class MyGroupAdapter extends RecyclerView.Adapter<MyGroupAdapter.ViewHolder> {

    private final Context context;
    private final List<GroupResponseDto> groups;
    private final UserViewModel userViewModel;


    // Constructor modification
    public MyGroupAdapter(UserViewModel userViewModel, Context context, List<GroupResponseDto> groups) {
        this.userViewModel = userViewModel;
        this.context = context;
        this.groups = groups;
    }

    @NonNull
    @Override
    public MyGroupAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater = LayoutInflater.from(context);
        View view = inflater.inflate(R.layout.item_group, parent, false);
        return new MyGroupAdapter.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull MyGroupAdapter.ViewHolder holder, int position) {
        GroupResponseDto group = groups.get(position);
        Log.d("Group", String.valueOf(group.getCreatorUserID()));
        userViewModel.getUserById(group.getCreatorUserID());

        userViewModel.getGroupCreatorUser().observeForever(user -> {
            holder.groupName.setText(group.getGroupName());
        });

        holder.groupInfo.setOnClickListener(view -> {
            if (context instanceof FragmentActivity) {
                FragmentActivity fragmentActivity = (FragmentActivity) context;
                GroupFragment groupFragment = GroupFragment.newInstance(group.getGroupId());
                fragmentActivity.getSupportFragmentManager().beginTransaction()
                        .replace(R.id.frameGroupContainer, groupFragment) // Remplace le fragment actuel
                        .addToBackStack(null) // Permet de revenir en arrière pour fermer ce fragment
                        .commit();
            }
        });
    }


    @Override
    public int getItemCount() {
        return groups.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView groupName;
        Button groupInfo;
        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            groupName = itemView.findViewById(R.id.GroupName);
            groupInfo = itemView.findViewById(R.id.groupInfo);
        }
    }

}
