import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/content/bloc/content_bloc.dart';
import 'package:netflix/ui/content/bloc/content_event.dart';
import 'package:netflix/ui/content/bloc/content_state.dart';
import 'package:netflix/ui/content/widgets/content_description.dart';
import 'package:netflix/ui/content/widgets/content_metrics.dart';
import 'package:netflix/ui/content/widgets/content_persons.dart';
import 'package:netflix/ui/content/widgets/content_ratings.dart';
import 'package:netflix/ui/content/widgets/content_short_summary.dart';
import 'package:netflix/ui/content/widgets/content_trailer.dart';

class ContentPage extends StatelessWidget {
  final int contentId;

  const ContentPage({super.key, required this.contentId});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) =>
              ContentBloc.createViaLocator()
                ..add(ContentPageOpened(contentId: contentId)),
      child: BlocConsumer<ContentBloc, ContentState>(
        listener: (context, state) {
          if (state.error.isNotEmpty) {
            ScaffoldMessenger.of(
              context,
            ).showSnackBar(SnackBar(content: Text(state.error)));
          }
        },
        builder: (context, state) {
          if (state.isLoading) {
            return Scaffold(
              appBar: AppBar(title: const Text('Загрузка...')),
              body: const Center(child: CircularProgressIndicator()),
            );
          }
          if (state.content == null) {
            return Scaffold(
              appBar: AppBar(title: const Text("Ошибка")),
              body: Center(child: const Text('Что-то пошло не так')),
            );
          }

          return Scaffold(
            appBar: AppBar(title: Text(state.content!.title)),
            body: Center(
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    ContentShortSummary(content: state.content!),
                    ContentDescription(description: state.content!.description),
                    ContentTrailer(trailerInfo: state.content!.trailerInfo),
                    ContentRatings(ratings: state.content!.ratings),
                    ContentPersons(
                      personsInContent: state.content!.personsInContent,
                    ),
                    ContentMetrics(views: state.contentViews)
                  ],
                ),
              ),
            ),
          );
        },
      ),
    );
  }
}
