#pragma once

#include <string>  // string
#include <utility> // pair
template<typename StringT>
class tokenizer {
public:
typedef StringT string_type;
typedef typename string_type::size_type size_type;
typedef std::pair<size_type,size_type> range_type;

	// コンストラクタ: 区切り文字の集合を引数に与える
	explicit tokenizer(const string_type& delim) : dlm_(delim) {}

	// 与えた文字列のトークン分割を開始する
	void start(const string_type& source) { src_ = source; reset(); }

	// startに与えた文字列のトークン分割を最初からやりなおす
	void reset() { rng_.first = 0; rng_.second = 0; }

	// トークンを切り出す。切り出すトークンがなければfalseを返す
	bool next() {
	  rng_.first = src_.find_first_not_of(dlm_, rng_.first+rng_.second);
	  if ( rng_.first == std::string::npos ) return false;
	  rng_.second = src_.find_first_of(dlm_, rng_.first);
	  if ( rng_.second == std::string::npos ) {
		rng_.second = src_.size() - rng_.first;
		return true;
	  }
	  rng_.second -= rng_.first;
	  return true;
	}

	// nextで切り出されたトークンを返す
	string_type token() const 
	  { return src_.substr(rng_.first,rng_.second); }

	// next切り出されたトークンの位置と長さを返す
	range_type range() const { return rng_; }

	// 一気に切り出してOutputIteratorに書き出す
	template<typename OutputIterator>
	OutputIterator split(OutputIterator out)
	  { while ( next() ) *out++ = token(); return out; }
	template<typename OutputIterator>
	OutputIterator split(const string_type& source, OutputIterator out) 
	  { start(source); return split(out); }

	private:
	string_type src_; // 分割対象となる文字列
	string_type dlm_; // 区切り文字の集合
	range_type  rng_; // トークンの位置と長さ
};
