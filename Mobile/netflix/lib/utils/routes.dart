class Routes {
  Routes._();

  static const main = '/';
  static const search = '/search';
  static const auth = '/auth';
  static const subscriptions = '/subscriptions';

  static const contentRouteName = 'content';
  static const contentRoutePath = '/content';
  static const contentTemplate = '/content/:id';


  static const profile = '/profile';

  static const profilePersonal = '$profile/$profilePersonalRelative';
  static const profilePersonalRelative = 'personal';

  static const profileFavorites = '$profile/$profileFavoritesRelative';
  static const profileFavoritesRelative = 'favorites';

  static const profileReviews = '$profile/$profileReviewsRelative';
  static const profileReviewsRelative = 'reviews';

  static const profileSubscriptions = '$profile/$profileSubscriptionsRelative';
  static const profileSubscriptionsRelative = 'subscriptions';

  static const profileSupportChat = '$profile/$profileSupportChatRelative';
  static const profileSupportChatRelative = 'support-chat';
}