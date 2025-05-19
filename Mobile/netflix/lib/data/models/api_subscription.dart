class ApiSubscription {
  final int id;
  final String name;
  final String description;
  final int maxResolution;
  final int price;

  ApiSubscription({
    required this.id,
    required this.name,
    required this.description,
    required this.maxResolution,
    required this.price,
  });

  ApiSubscription.fromMap(Map<String, dynamic> map)
      : id = map['id'],
        name = map['name'],
        description = map['description'],
        maxResolution = map['maxResolution'],
        price = map['price'];
}