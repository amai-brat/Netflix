import 'package:netflix/domain/models/content/trailer_info.dart';

class ApiTrailerInfo {
  final String url;
  final String name;

  const ApiTrailerInfo({required this.url, required this.name});

  ApiTrailerInfo.fromMap(Map<String, dynamic> map)
    : url = map['url'],
      name = map['name'];

  TrailerInfo toTrailerInfo() => TrailerInfo(url: url, name: name);
}
