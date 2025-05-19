import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/ui/profile/tabs/reviews/widgets/PaginationWidget.dart';
import 'bloc/reviews_bloc.dart';

enum ReviewSortType { date, score }

extension on ReviewSortType {
  String get title => switch (this) {
    ReviewSortType.date => 'По дате',
    ReviewSortType.score => 'По оценке',
  };

  String get backendValue => switch (this) {
    ReviewSortType.date => 'date',
    ReviewSortType.score => 'score',
  };
}

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
  ReviewSortType _selectedSort = ReviewSortType.date;

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

  void _onSearchSubmitted() {
    final text = _searchController.text.trim();
    context.read<ReviewsBloc>().add(
      LoadReviewsEvent(
        search: text,
        sort: _selectedSort.backendValue,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Рецензии')),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(12),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: _searchController,
                    onSubmitted: (_) => _onSearchSubmitted(),
                    textInputAction: TextInputAction.done,
                    decoration: InputDecoration(
                      labelText: 'Поиск',
                      suffixIcon: IconButton(
                        icon: const Icon(Icons.search),
                        onPressed: _onSearchSubmitted,
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 12),
                DropdownButton<ReviewSortType>(
                  value: _selectedSort,
                  onChanged: (newValue) {
                    if (newValue != null) {
                      setState(() => _selectedSort = newValue);
                      _onSearchSubmitted();
                    }
                  },
                  items: ReviewSortType.values
                      .map((e) => DropdownMenuItem(
                    value: e,
                    child: Text(e.title),
                  ))
                      .toList(),
                ),
              ],
            ),
          ),
          Expanded(
            child: BlocBuilder<ReviewsBloc, ReviewsState>(
              builder: (context, state) {
                return switch (state) {
                  ReviewsLoading() =>
                  const Center(child: CircularProgressIndicator()),
                  ReviewsError(:final message) =>
                      Center(child: Text('Ошибка: $message')),
                  ReviewsNormal(:final reviews, :final currentPage, :final totalPages, :final isLoading) =>
                      Column(
                        children: [
                          if (isLoading) const LinearProgressIndicator(),
                          Expanded(child: _ReviewsList(reviews: reviews)),
                          PaginationWidget(
                            currentPage: currentPage,
                            totalPages: totalPages,
                            onPageSelected: (page) => context.read<ReviewsBloc>().add(
                              ChangePageEvent(page),
                            ),
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
      itemBuilder: (_, i) => _ReviewTile(review: reviews[i]),
    );
  }
}

class _ReviewTile extends StatefulWidget {
  final UserReview review;

  const _ReviewTile({required this.review});

  @override
  State<_ReviewTile> createState() => _ReviewTileState();
}

class _ReviewTileState extends State<_ReviewTile> {
  bool _expanded = false;

  @override
  Widget build(BuildContext context) {
    final r = widget.review;

    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
      child: Padding(
        padding: const EdgeInsets.all(12),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Оценка
            Text(
              '${r.score}',
              style: Theme.of(context)
                  .textTheme
                  .headlineMedium
                  ?.copyWith(color:
                    r.isPositive ? Colors.green: Colors.red
                  ),
            ),
            const SizedBox(width: 12),
            // Информация и текст
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(r.contentName,
                      style: const TextStyle(fontWeight: FontWeight.bold)),
                  Text(
                    _formatDate(r.writtenAt),
                    style: const TextStyle(color: Colors.grey),
                  ),
                  const SizedBox(height: 6),
                  GestureDetector(
                    onTap: () => setState(() => _expanded = !_expanded),
                    child: Text(
                      _expanded || r.text.length <= 100
                          ? r.text
                          : '${r.text.substring(0, 100)}...',
                      style: const TextStyle(fontSize: 14),
                    ),
                  ),
                  if (r.text.length > 100)
                    TextButton(
                      onPressed: () => setState(() => _expanded = !_expanded),
                      child: Text(_expanded ? 'Скрыть' : 'Показать больше'),
                    ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  String _formatDate(DateTime date) {
    return '${date.day.toString().padLeft(2, '0')}.${date.month.toString().padLeft(2, '0')}.${date.year}';
  }
}