import 'package:netflix/domain/models/content/age_ratings.dart';

class ApiAgeRatingsDto {
  final int age;
  final String ageMpaa;

  const ApiAgeRatingsDto({required this.age, required this.ageMpaa});

  ApiAgeRatingsDto.fromMap(Map<String, dynamic> map)
    : age = map['age'],
      ageMpaa = map['ageMpaa'];

  AgeRatings toAgeRatings() => AgeRatings(age: age, ageMpaa: ageMpaa);
}
