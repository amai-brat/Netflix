import 'package:flutter/material.dart';

class PaginationWidget extends StatelessWidget {
  final int currentPage;
  final int totalPages;
  final void Function(int page) onPageSelected;

  const PaginationWidget({
    required this.currentPage,
    required this.totalPages,
    required this.onPageSelected,
  });

  @override
  Widget build(BuildContext context) {
    List<Widget> pageButtons = [];

    void addPage(int page) {
      pageButtons.add(
        SizedBox(
          height: 32,
          child: ElevatedButton(
            onPressed: () => onPageSelected(page),
            style: ElevatedButton.styleFrom(
              padding: const EdgeInsets.symmetric(horizontal: 12),
              minimumSize: Size.zero,
              tapTargetSize: MaterialTapTargetSize.shrinkWrap,
              backgroundColor: page == currentPage ? Colors.blue : null,
            ),
            child: Text(
              page.toString(),
              style: const TextStyle(fontSize: 14),
            ),
          ),
        ),
      );
    }

    void addEllipsis() {
      pageButtons.add(const Padding(
        padding: EdgeInsets.symmetric(horizontal: 6),
        child: Text('...', style: TextStyle(fontSize: 14)),
      ));
    }

    if (totalPages <= 1) return const SizedBox();

    addPage(1);

    if (currentPage > 3) {
      addEllipsis();
    }

    for (int i = currentPage - 1; i <= currentPage + 1; i++) {
      if (i > 1 && i < totalPages) {
        addPage(i);
      }
    }

    if (currentPage < totalPages - 2) {
      addEllipsis();
    }

    if (totalPages > 1) {
      addPage(totalPages);
    }

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Wrap(
        spacing: 6,
        runSpacing: 6,
        children: pageButtons,
      ),
    );
  }
}