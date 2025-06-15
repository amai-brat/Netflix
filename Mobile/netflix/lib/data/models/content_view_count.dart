class ContentViewCount {
  final int contentId;
  final int views;

  ContentViewCount({required this.contentId, required this.views});

  ContentViewCount.fromMap(Map<String, dynamic> map)
      : contentId = map['contentId'],
        views = map['views'];
}