import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_event.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_state.dart';
import 'package:netflix/ui/profile/tabs/favorites/components/content/favorite_card.dart';

class FavoriteList extends StatelessWidget {
  const FavoriteList({super.key});

  @override
  Widget build(BuildContext context) {
    return Center(
      child: BlocBuilder<FavoriteBloc, FavoriteState>(
        builder: (context, state) {
          if (state.isLoading) {
            return CircularProgressIndicator();
          }else{
            final favorites = state.favorites;
            final ctx = context.read<FavoriteBloc>();

            return NotificationListener<ScrollNotification>(
              onNotification: (notification) {
                if (notification.metrics.pixels >= notification.metrics.maxScrollExtent - 200) {
                  ctx.add(LoadFavorite());
                }
                return false;
              },
              child: ListView.builder(
                padding: const EdgeInsets.symmetric(horizontal: 12.0, vertical: 8.0),
                itemCount: favorites.length + (state.hasMore ? 1 : 0),
                itemBuilder: (_, index) {
                  if (index >= favorites.length) {
                    return const Padding(
                      padding: EdgeInsets.symmetric(vertical: 16.0),
                      child: Center(child: CircularProgressIndicator()),
                    );
                  }

                  return Padding(
                    padding: const EdgeInsets.only(bottom: 12.0),
                    child: FavoriteCard(favorite: favorites[index]),
                  );
                },
              ),
            );
          }
        },
      )
    );
  }
}