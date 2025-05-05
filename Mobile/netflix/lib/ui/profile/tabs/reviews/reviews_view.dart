import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/ui/profile/tabs/reviews/widgets/PaginationWidget.dart';
import 'bloc/reviews_bloc.dart';

class ReviewsView extends StatelessWidget {
  const ReviewsView({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => ReviewsBloc.createViaLocator()..add(LoadReviewsEvent()),
      child: const _ReviewsViewBody(),
    );
  }
}

class _ReviewsViewBody extends StatefulWidget {
  const _ReviewsViewBody({super.key});

  @override
  State<_ReviewsViewBody> createState() => _ReviewsViewBodyState();
}

class _ReviewsViewBodyState extends State<_ReviewsViewBody> {
  late final TextEditingController _searchController;

  @override
  void initState() {
    super.initState();
    _searchController = TextEditingController();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  void _onSearchSubmitted(String value) {
    context.read<ReviewsBloc>().add(LoadReviewsEvent(search: value));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Рецензии')),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(12),
            child: TextField(
              controller: _searchController,
              onSubmitted: _onSearchSubmitted,
              textInputAction: TextInputAction.done,
              decoration: const InputDecoration(
                labelText: 'Поиск',
                suffixIcon: Icon(Icons.search),
              ),
            ),
          ),
          Expanded(
            child: BlocBuilder<ReviewsBloc, ReviewsState>(
              builder: (context, state) {
                return switch (state) {
                  ReviewsLoading() => const Center(child: CircularProgressIndicator()),
                  ReviewsError(:final message) => Center(child: Text('Ошибка: $message')),
                  ReviewsNormal(:final reviews, :final currentPage, :final totalPages, :final isLoading) => Column(
                    children: [
                      if (isLoading) const LinearProgressIndicator(),
                      Expanded(child: _ReviewsList(reviews: reviews)),
                      PaginationWidget(
                        currentPage: currentPage,
                        totalPages: totalPages,
                        onPageSelected: (page) =>
                            context.read<ReviewsBloc>().add(ChangePageEvent(page)),
                      ),
                    ],
                  ),
                };
              },
            ),
          ),
        ],
      ),
    );
  }
}

class _ReviewsList extends StatelessWidget {
  final List<UserReview> reviews;

  const _ReviewsList({required this.reviews});

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      itemCount: reviews.length,
      itemBuilder: (_, i) {
        final r = reviews[i];
        return ListTile(
          title: Text('${r.name} (${r.score}/5)'),
          subtitle: Text(r.text),
          trailing: Text(r.contentName),
        );
      },
    );
  }
}