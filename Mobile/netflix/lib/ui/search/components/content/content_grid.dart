import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import '../../../core/widgets/content_card.dart';

class ContentGrid extends StatelessWidget {
  const ContentGrid({super.key});

  @override
  Widget build(BuildContext context) {
    return Center(
      child: BlocBuilder<SearchBloc, SearchState>(
        builder: (context, state) {
          if (state.isLoading) {
            return CircularProgressIndicator();
          }else{
            final contents = state.contents;
            final ctx = context.read<SearchBloc>();

            return NotificationListener<ScrollNotification>(
                onNotification: (notification) {
                  if (notification.metrics.pixels >= notification.metrics.maxScrollExtent - 200) {
                    ctx.add(LoadContent());
                  }
                  return false;
                },
                child: GridView.builder(
                  padding: EdgeInsets.symmetric(horizontal: 8.0),
                  gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                    crossAxisCount: 2,
                    childAspectRatio: 0.7,
                    crossAxisSpacing: 4,
                    mainAxisSpacing: 4
                  ),
                  itemCount: contents.length + (state.hasMore ? 1 : 0),
                  itemBuilder: (_, i) {
                    if (i >= state.contents.length) {
                      return const Center(child: CircularProgressIndicator());
                    }
                    return ContentCard(content: state.contents[i]);
                  },
                )
            );
          }
        }
      )
    );
  }
}
